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

namespace Application.ParameterChecks
{
    public class List
    {
        public class GetDataReferencesQuery : IRequest<ListResult<DataReferenceDTO>>
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public string Category { get; set; }
            public string SearchQuery { get; set; }

            public GetDataReferencesQuery(int pageNumber, int pageSize, string category, string searchQuery)
            {
                PageNumber = pageNumber;
                PageSize = pageSize;
                Category = category;
                SearchQuery = searchQuery;
            }
        }
        public class Query : IRequest<ListResult<ParameterCheckDTO>>
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

        public class Handler : IRequestHandler<Query, ListResult<ParameterCheckDTO>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ListResult<ParameterCheckDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                IQueryable<ParameterCheck> query = _context.ParameterChecks
                     .Include(dt => dt.DataReference);
                if (!string.IsNullOrEmpty(request.SearchQuery))
                {
                    if (request.Category == "All" && !string.IsNullOrEmpty(request.SearchQuery))
                    {
                        query = query.Where(dl =>
                            dl.Description.Contains(request.SearchQuery) ||
                            dl.DataReference.RefereceName.Contains(request.SearchQuery) 

                        );
                        query = query.OrderBy(p => p.DataReferenceId).ThenBy(p => p.Order);

                    }
                    else if (request.Category == "All" && string.IsNullOrEmpty(request.SearchQuery))
                    {
                        query = query.OrderBy(p => p.DataReferenceId).ThenBy(p => p.Order);
                    }
                    else
                    {
                        switch (request.Category)
                        {
                            case "Description":
                                query = query.Where(dt => dt.Description.Contains(request.SearchQuery));
                                break;
                            case "RefereceName":
                                query = query.Where(dt => dt.DataReference.RefereceName.Contains(request.SearchQuery));
                                break;
                          
                            default:
                                // Handle unsupported category here
                                break;
                        }
                        query = query.OrderBy(p => p.DataReferenceId).ThenBy(p => p.Order);
                    }
                }
                else
                {
                    query = query.OrderBy(p => p.DataReferenceId).ThenBy(p => p.Order);
                }



                var totalItems = await query.CountAsync();
                var currentPage = request.PageNumber;
                var pageSize = request.PageSize;

                var dataParam = await query
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var parameterCheck = _mapper.Map<List<ParameterCheckDTO>>(dataParam);

                return new ListResult<ParameterCheckDTO>
                {
                    Items = parameterCheck,
                    TotalItems = totalItems,
                    CurrentPage = currentPage,
                    PageSize = pageSize
                };
            }
        }
    }
}