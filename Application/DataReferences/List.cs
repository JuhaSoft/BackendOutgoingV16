using AutoMapper;
using Common.DTOs;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DataReferences
{
    public class List
    {
        public class Query : IRequest<ListResult<DataReferenceDTO>>
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public string Category { get; set; } // Tambahkan properti Category
            public string SearchQuery { get; set; } // Tambahkan properti SearchQuery
        }

        public class ListResult<T>
        {
            public List<T> Items { get; set; }
            public int TotalItems { get; set; }
            public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
            public int CurrentPage { get; set; }
            public int PageSize { get; set; }
        }

        public class Handler : IRequestHandler<Query, ListResult<DataReferenceDTO>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ListResult<DataReferenceDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                IQueryable<DataReference> query = _context.DataReferences
                     .Include(dt => dt.LastStationID);
                if (!string.IsNullOrEmpty(request.SearchQuery))
                {
                    if (request.Category == "All" && !string.IsNullOrEmpty(request.SearchQuery))
                    {
                        query = query.Where(dl =>
                            dl.RefereceName.Contains(request.SearchQuery) ||
                            dl.LastStationID.StationID.Contains(request.SearchQuery) 

                        );
                        query = query.OrderByDescending(dt => dt.RefereceName);
                    }
                    else if (request.Category == "All" && string.IsNullOrEmpty(request.SearchQuery))
                    {
                        query = query.OrderByDescending(dt => dt.StationID);
                    }
                    else
                    {
                        switch (request.Category)
                        {
                            case "RefereceName":
                                query = query.Where(dt => dt.RefereceName.Contains(request.SearchQuery));
                                break;
                            case "StationID":
                                query = query.Where(dt => dt.LastStationID.StationID.Contains(request.SearchQuery));
                                break;
                            

                            default:
                                // Handle unsupported category here
                                break;
                        }
                        query = query.OrderByDescending(dt => dt.RefereceName);
                    }
                }
                else
                {
                    query = query.OrderByDescending(dt => dt.RefereceName);
                }



                var totalItems = await query.CountAsync();
                var currentPage = request.PageNumber;
                var pageSize = request.PageSize;

                var dataref = await query
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var dataReference = _mapper.Map<List<DataReferenceDTO>>(dataref);

                return new ListResult<DataReferenceDTO>
                {
                    Items = dataReference,
                    TotalItems = totalItems,
                    CurrentPage = currentPage,
                    PageSize = pageSize
                };
            }
        }
    }
}