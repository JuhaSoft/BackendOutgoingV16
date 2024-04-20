using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DataReferences
{
    class RefDetail
    {
        public class Query : IRequest<DataReference>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, DataReference>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                this._context = context;
            }
            public async Task<bool> IsUnique(Guid Id)
            {
                return await _context.DataReferences.AnyAsync(u => u.Id == Id);
            }
            public async Task<DataReference> Handle(Query request, CancellationToken cancellationToken)
            {

                if (!await IsUnique(request.Id))
                {

                    throw new Exception("Id  :'" + request.Id + "' Tidak ditemukan.");
                }
                return await _context.DataReferences
                    .Include(dt => dt.LastStationID)
                    .FirstOrDefaultAsync(u => u.Id == request.Id);
            }

            
        }
    }
}
