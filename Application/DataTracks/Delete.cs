using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataTracks
{
    public class Delete
    {
        public class Command : IRequest 
        {
            public Guid Id { get; set; }
        }
        public class Handle : IRequestHandler<Command>
        {
        private readonly DataContext _context;
            public Handle(DataContext context)
            {
            this._context = context;
            }
     public async Task<bool> IsDataTrackUnique(Guid id)
            {
                return await _context.DataTracks.AnyAsync(u => u.Id == id);
            }
            async Task<Unit> IRequestHandler<Command, Unit>.Handle(Command request, CancellationToken cancellationToken)
            {
                var DataTrack= await _context.DataTracks.FindAsync(request.Id);
                 if (!await IsDataTrackUnique(request.Id))
                {
                   
                    throw new Exception("Data   Tidak ditemukan.");
                }
                _context.Remove(DataTrack);
                await _context.SaveChangesAsync();
                return Unit.Value;
            }

            
        }
    }
}