using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataContrplTypes
{
    public class List
    {
        public class PCQuery : IRequest<List<DataContrplType>>
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
        }
        public class Handler : IRequestHandler<PCQuery, List<DataContrplType>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                this._context = context;
            }
            public async Task<List<DataContrplType>> Handle(PCQuery request, CancellationToken cancellationToken)
            {
                if (request.PageNumber <= 0)
                {
                return await _context.DataContrplTypes.ToListAsync();
                   
                }
                else
                {
                
                    return await _context.DataContrplTypes
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .ToListAsync();
                }
                //throw new NotImplementedException();
            }


        }
    }
}