using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.LastStationIDs
{
    class Detail
    {
        public class Query : IRequest<LastStationID>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, LastStationID>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                this._context = context;
            }
            public async Task<bool> IsUnique(Guid Id)
            {
                return await _context.LastStationIDs.AnyAsync(u => u.Id == Id);
            }
            public async Task<LastStationID> Handle(Query request, CancellationToken cancellationToken)
            {
                
                if (!await IsUnique(request.Id))
                {
                    
                    throw new Exception("Id  :'" + request.Id + "' Tidak ditemukan.");
                }
                return await _context.LastStationIDs
                    .Include(dt => dt.DataLine)
                    .FirstOrDefaultAsync(u => u.Id == request.Id);
            }

           
        }
    }
}
