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
    public class DetailByRefrence
    {
        public class Query : IRequest<ParameterCheck> // Mengubah tipe balikan menjadi List<ParameterCheck>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, ParameterCheck> // Mengubah tipe balikan menjadi List<ParameterCheck>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<bool> IsPCUnique(Guid id)
            {
                return await _context.DataReferences.AnyAsync(u => u.Id == id);
            }

            public async Task<ParameterCheck> Handle(Query request, CancellationToken cancellationToken) // Mengubah tipe balikan menjadi List<ParameterCheck>
            {
                if (!await IsPCUnique(request.Id))
                {
                    throw new Exception("Id :'" + request.Id + "' Tidak ditemukan.");
                }

                return await _context.ParameterChecks
                 .Include(pc => pc.ParameterCheckErrorMessages)
                    .ThenInclude(pcem => pcem.ErrorMessage)
                .Include(pc => pc.DataReferenceParameterChecks)
               .FirstOrDefaultAsync(u => u.DataReferenceParameterChecks.Any(drpc => drpc.DataReferenceId == request.Id));


            }
        }
    }
}
