using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Model;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.WorkOrders
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
                return await _context.WorkOrders.AnyAsync(u => u.Id == id);
            }
            async Task<Unit> IRequestHandler<Command, Unit>.Handle(Command request, CancellationToken cancellationToken)
            {
                var wo= await _context.WorkOrders.FindAsync(request.Id);
                 if (!await IsUnique(request.Id))
                {
                   
                    throw new Exception("Data WO  Tidak ditemukan.");
                }
                _context.Remove(wo);
                await _context.SaveChangesAsync();
                return Unit.Value;
            }

          
        } 
    }
}