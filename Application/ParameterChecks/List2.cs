using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Persistence;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Common.DTOs;

namespace Application.ParameterChecks
{
    public class List2
    {
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
                                                    .Include(pc => pc.ParameterCheckErrorMessages)
                                                        .ThenInclude(pcem => pcem.ErrorMessage)
                                                    .Include(pc => pc.DataReferenceParameterChecks);
                if (!string.IsNullOrEmpty(request.SearchQuery))
                {
                    if (request.Category == "All" && !string.IsNullOrEmpty(request.SearchQuery))
                    {
                        query = query.Where(pc =>
                            pc.Description.Contains(request.SearchQuery) ||
                            pc.DataReferenceParameterChecks.Any(drpc => drpc.DataReference.RefereceName.Contains(request.SearchQuery)) ||
                             pc.ParameterCheckErrorMessages.Any(pcem => pcem.ErrorMessage.ErrorDescription.Contains(request.SearchQuery))
                        );
                        query = query.OrderByDescending(wo => wo.Description);
                    }
                    else if (request.Category == "All" && string.IsNullOrEmpty(request.SearchQuery))
                    {
                        query = query.OrderByDescending(wo => wo.Description);
                    }
                    else
                    {
                        switch (request.Category)
                        {
                            case "Description":
                                query = query.Where(wo => wo.Description.Contains(request.SearchQuery));
                                break;
                            
                            default:
                                // Handle unsupported category here
                                break;
                        }
                        query = query.OrderByDescending(wo => wo.Description);
                    }
                }
                else
                {
                    query = query.OrderByDescending(wo => wo.Description);
                }
               
                

                var totalItems = await query.CountAsync();
                var currentPage = request.PageNumber;
                var pageSize = request.PageSize;

                var workOrders = await query
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var parameterchecks = _mapper.Map<List<ParameterCheckDTO>>(workOrders);

                return new ListResult<ParameterCheckDTO>
                {
                    Items = parameterchecks,
                    TotalItems = totalItems,
                    CurrentPage = currentPage,
                    PageSize = pageSize
                };
            }
        }
    }
}