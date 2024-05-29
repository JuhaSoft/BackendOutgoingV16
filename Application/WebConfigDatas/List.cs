using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.WebConfigDatas
{
    public class List
    {
        public class Query : IRequest<ListResult<WebConfigData>>
        {

        }

        public class ListResult<T>
        {
            public List<T> Items { get; set; }

        }

        public class Handler : IRequestHandler<Query, ListResult<WebConfigData>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ListResult<WebConfigData>> Handle(Query request, CancellationToken cancellationToken)
            {
                IQueryable<WebConfigData> query = _context.WebConfigDatas;


                var dataConfig = await query
                    .ToListAsync();



                return new ListResult<WebConfigData>
                {
                    Items = dataConfig
                };
            }
        }

    }
}