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

namespace Application.ParameterChecks
{
    public class List
    {
        public class GetDataReferencesQuery : IRequest<ListResult<DataReference>>
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
        public class Query : IRequest<ListResult<ParameterCheck>>
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

        public class Handler : IRequestHandler<Query, ListResult<ParameterCheck>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ListResult<ParameterCheck>> Handle(Query request, CancellationToken cancellationToken)
            {
                IQueryable<ParameterCheck> query = _context.ParameterChecks
                                                    .Include(pc => pc.ParameterCheckErrorMessages)
                                                        .ThenInclude(pcem => pcem.ErrorMessage)
                                                    .Include(pc => pc.DataReferenceParameterChecks);
                //var totalItems = await query.CountAsync();
                IQueryable<ParameterCheck> filteredQuery;

                if (!string.IsNullOrEmpty(request.SearchQuery))
                {
                    if (request.Category == "All" && !string.IsNullOrEmpty(request.SearchQuery))
                    {
                        filteredQuery = query.Where(pc =>
                            pc.Description.Contains(request.SearchQuery) ||
                            pc.DataReferenceParameterChecks.Any(drpc => drpc.DataReference.RefereceName.Contains(request.SearchQuery)) ||
                            pc.ParameterCheckErrorMessages.Any(pcem => pcem.ErrorMessage.ErrorDescription.Contains(request.SearchQuery))
                        );
                    }
                    else if (request.Category == "All" && string.IsNullOrEmpty(request.SearchQuery))
                    {
                        filteredQuery = query;
                    }
                    else
                    {
                        switch (request.Category)
                        {
                            case "Description":
                                filteredQuery = query.Where(pc => pc.Description.Contains(request.SearchQuery));
                                break;
                            case "RefereceName":
                                filteredQuery = query.Where(pc => pc.DataReferenceParameterChecks.Any(drpc => drpc.DataReference.RefereceName.Contains(request.SearchQuery)));
                                break;
                            case "ErrorDescription":
                                filteredQuery = query.Where(pc => pc.ParameterCheckErrorMessages.Any(pcem => pcem.ErrorMessage.ErrorDescription.Contains(request.SearchQuery)));
                                break;
                            default:
                                filteredQuery = query;
                                break;
                        }
                    }
                }
                else
                {
                    filteredQuery = query;
                }

                var totalItems = await filteredQuery.CountAsync();

                var currentPage = request.PageNumber;
                var pageSize = request.PageSize;

                var dataParam = filteredQuery
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .AsEnumerable() // Evaluasi di sisi klien
                    .OrderBy(p => p.DataReferenceParameterChecks?.Count() ?? 0)
                        .ThenBy(p => p.Order == 0 ? 0 : p.Order)
                    .ToList();

                var parameterCheck = _mapper.Map<List<ParameterCheck>>(dataParam);

                return new ListResult<ParameterCheck>
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