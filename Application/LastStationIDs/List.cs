using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.DTOs.LastStationID;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using static Application.WorkOrders.List;

namespace Application.LastStationIDs
{
    public class List
    {
        public class Query : IRequest<ListResult<LastStationIDDTO>>
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

        public class Handler : IRequestHandler<Query, ListResult<LastStationIDDTO>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ListResult<LastStationIDDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                IQueryable<LastStationID> query = _context.LastStationIDs
                     .Include(dt => dt.DataLine);
                if (!string.IsNullOrEmpty(request.SearchQuery))
                {
                    if (request.Category == "All" && !string.IsNullOrEmpty(request.SearchQuery))
                    {
                        query = query.Where(dl =>
                            dl.StationID.Contains(request.SearchQuery) ||
                            dl.StationName.Contains(request.SearchQuery) ||
                            dl.DataLine.LineName.Contains(request.SearchQuery)

                        );
                        query = query.OrderByDescending(dt => dt.LineId);
                    }
                    else if (request.Category == "All" && string.IsNullOrEmpty(request.SearchQuery))
                    {
                        query = query.OrderByDescending(dt => dt.StationID);
                    }
                    else
                    {
                        switch (request.Category)
                        {
                            case "StationID":
                                query = query.Where(dt => dt.StationID.Contains(request.SearchQuery));
                                break;
                            case "StationName":
                                query = query.Where(dt => dt.StationName.Contains(request.SearchQuery));
                                break;
                            case "LineName":
                                query = query.Where(dt => dt.DataLine.LineName.Contains(request.SearchQuery));
                                break;

                            default:
                                // Handle unsupported category here
                                break;
                        }
                        query = query.OrderByDescending(dt => dt.LineId);
                    }
                }
                else
                {
                    query = query.OrderByDescending(dt => dt.LineId);
                }



                var totalItems = await query.CountAsync();
                var currentPage = request.PageNumber;
                var pageSize = request.PageSize;

                var dataLine = await query
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var lastStationID = _mapper.Map<List<LastStationIDDTO>>(dataLine);

                return new ListResult<LastStationIDDTO>
                {
                    Items = lastStationID,
                    TotalItems = totalItems,
                    CurrentPage = currentPage,
                    PageSize = pageSize
                };
            }
        }
    }
}