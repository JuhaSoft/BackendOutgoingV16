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
    public class List
    {
        public class Query : IRequest<List<SelectOption>>
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
        }
        public class Handler : IRequestHandler<Query, List<SelectOption>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                this._context = context;
            }
            public async Task<List<SelectOption>> Handle(Query request, CancellationToken cancellationToken)
            {

                if (request.PageNumber <= 0)
                {
                    return await _context.SelectOptions.ToListAsync();
                }
                else
                {
                    // Jika pageNumber > 0, maka gunakan metode paging
                    return await _context.SelectOptions
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .ToListAsync();
                }

            }


        }
    }
}