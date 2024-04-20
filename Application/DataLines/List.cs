using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.DTOs;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataLines
{
   public class List
    {
        public class Query : IRequest<ListResult<DataLineDTO>>
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

        public class Handler : IRequestHandler<Query, ListResult<DataLineDTO>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ListResult<DataLineDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                IQueryable<DataLine> query = _context.DataLines;
                if (!string.IsNullOrEmpty(request.SearchQuery))
                {
                    if (request.Category == "All" && !string.IsNullOrEmpty(request.SearchQuery))
                    {
                        query = query.Where(dl =>
                            dl.LineId.Contains(request.SearchQuery) ||
                            dl.LineName.Contains(request.SearchQuery) 
                            
                        );
                        query = query.OrderByDescending(dt => dt.LineId);
                    }
                    else if (request.Category == "All" && string.IsNullOrEmpty(request.SearchQuery))
                    {
                        query = query.OrderByDescending(dt => dt.LineName);
                    }
                    else
                    {
                        switch (request.Category)
                        {
                            case "LineId":
                                query = query.Where(dt => dt.LineId.Contains(request.SearchQuery));
                                break;
                            case "LineName":
                                query = query.Where(dt => dt.LineName.Contains(request.SearchQuery));
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

                var dataLineDTO = _mapper.Map<List<DataLineDTO>>(dataLine);

                return new ListResult<DataLineDTO>
                {
                    Items = dataLineDTO,
                    TotalItems = totalItems,
                    CurrentPage = currentPage,
                    PageSize = pageSize
                };
            }
        }
    }
}