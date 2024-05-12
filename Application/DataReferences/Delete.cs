using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataReferences
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
        public async Task<bool> IsUnique(Guid id)
            {
                return await _context.DataReferences.AnyAsync(u => u.Id == id);
            }
            async Task<Unit> IRequestHandler<Command, Unit>.Handle(Command request, CancellationToken cancellationToken)
            {
                var dataRef= await _context.DataReferences.FindAsync(request.Id);
                 if (!await IsUnique(request.Id))
                {
                   
                    throw new Exception("Data   Tidak ditemukan.");
                }
                _context.Remove(dataRef);
                var existingChecks = _context.DataReferenceParameterChecks
                        .Where(c => c.DataReferenceId == request.Id);
                _context.DataReferenceParameterChecks.RemoveRange(existingChecks);
                await _context.SaveChangesAsync();
                return Unit.Value;
            }

            
        }
    }
}