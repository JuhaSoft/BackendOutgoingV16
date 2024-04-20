using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CSelectOptions
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
         public async Task<bool> IsSOUnique(Guid id)
            {
                return await _context.SelectOptions.AnyAsync(u => u.Id == id);
            }
            async Task<Unit> IRequestHandler<Command, Unit>.Handle(Command request, CancellationToken cancellationToken)
            {
                var selectOption= await _context.SelectOptions.FindAsync(request.Id);
                 if (!await IsSOUnique(request.Id))
                {
                   
                    throw new Exception("Data   Tidak ditemukan.");
                }
                _context.Remove(selectOption);
                await _context.SaveChangesAsync();
                return Unit.Value;
            }

            
        }
    }
}