using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DataTrackCheckings
{
    public class Linst
    {
        public class DtQuery : IRequest<List<DataTrackChecking>> {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
         }
        public class Handler : IRequestHandler<DtQuery, List<DataTrackChecking>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                this._context = context;
            }
            public async Task<List<DataTrackChecking>> Handle(DtQuery request, CancellationToken cancellationToken)
            {
                if (request.PageNumber <= 0)
                {
                    return await _context.DataTrackCheckings
                    .Include(dt => dt.ParameterChecks)
                    .ToListAsync();
                }
                else
                {
                
                    return await _context.DataTrackCheckings
                    .Include(dt => dt.ParameterChecks)
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .ToListAsync();
                }
               
                //throw new NotImplementedException();
            }
        }

    }
}