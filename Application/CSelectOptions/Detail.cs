using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CSelectOptions
{
    public class Detail
    {
         public class Query : IRequest<SelectOption>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, SelectOption>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                this._context = context;
            }
            public async Task<bool> IsUnique(Guid id)
            {
                return await _context.SelectOptions.AnyAsync(u => u.Id == id);
            }
            public async Task<SelectOption> Handle(Query request, CancellationToken cancellationToken)
            {
                
                if (!await IsUnique(request.Id))
                {
                    
                    throw new Exception("Id :'" + request.Id + "' Tidak ditemukan.");
                }
                return await _context.SelectOptions.FirstOrDefaultAsync(u => u.Id == request.Id);
            }

            
        }
    }
}