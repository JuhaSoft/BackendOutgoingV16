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
        public class Query : IRequest<List<ParameterCheck>> // Mengubah tipe balikan menjadi List<ParameterCheck>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<ParameterCheck>> // Mengubah tipe balikan menjadi List<ParameterCheck>
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

            public async Task<List<ParameterCheck>> Handle(Query request, CancellationToken cancellationToken) // Mengubah tipe balikan menjadi List<ParameterCheck>
            {
                if (!await IsPCUnique(request.Id))
                {
                    throw new Exception("Id :'" + request.Id + "' Tidak ditemukan.");
                }

                return await _context.ParameterChecks
                    .Include(dt => dt.DataReference)
                    .Where(u => u.DataReferenceId == request.Id)
                    .ToListAsync(); // Mengubah metode dari FirstOrDefaultAsync menjadi ToListAsync
            }
        }
    }
}
