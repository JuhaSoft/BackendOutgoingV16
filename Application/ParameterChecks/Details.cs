using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ParameterChecks
{
    public class Details
    {
        public class Query : IRequest<ParameterCheck>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, ParameterCheck>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                this._context = context;
            }
            public async Task<bool> IsPCUnique(Guid id)
            {
                return await _context.ParameterChecks.AnyAsync(u => u.Id == id);
            }
            public async Task<ParameterCheck> Handle(Query request, CancellationToken cancellationToken)
            {
                
                if (!await IsPCUnique(request.Id))
                {
                    
                    throw new Exception("Id :'" + request.Id + "' Tidak ditemukan.");
                }
                return await _context.ParameterChecks
                 .Include(dt => dt.DataReference)
                .FirstOrDefaultAsync(u => u.Id == request.Id);
            }

            
        }
    }
}