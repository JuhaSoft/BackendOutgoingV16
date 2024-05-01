using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataReferences
{
    public class DetailByName
    {
        public class Query : IRequest<DataReference>
        {
            public string RefereceName { get; set; }
        }
        public class Handler : IRequestHandler<Query, DataReference>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                this._context = context;
            }
            public async Task<bool> IsUnique(string  Id)
            {
                return await _context.DataReferences.AnyAsync(u => u.RefereceName == Id);
            }
            public async Task<DataReference> Handle(Query request, CancellationToken cancellationToken)
            {
                
                if (!await IsUnique(request.RefereceName))
                {
                    
                    throw new Exception("Id  :'" + request.RefereceName + "' Tidak ditemukan.");
                }
                return await _context.DataReferences
                     .Include(dt => dt.LastStationID)
                     .ThenInclude(dt => dt.DataLine)
                    .FirstOrDefaultAsync(u => u.RefereceName == request.RefereceName);
            }

            
        }
    }
}