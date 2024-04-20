// Di dalam proyek Application
using AutoMapper;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.DTOs;
namespace Application.WorkOrders
{
    public class List
    {
        public class Query : IRequest<ListResult<WorkOrderDto>>
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

        public class Handler : IRequestHandler<Query, ListResult<WorkOrderDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ListResult<WorkOrderDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                IQueryable<WorkOrder> query = _context.WorkOrders.Include(wo => wo.User);
                if (!string.IsNullOrEmpty(request.SearchQuery))
                {
                    if (request.Category == "All" && !string.IsNullOrEmpty(request.SearchQuery))
                    {
                        query = query.Where(wo =>
                            wo.WoNumber.Contains(request.SearchQuery) ||
                            wo.SONumber.Contains(request.SearchQuery) ||
                            wo.WoReferenceID.Contains(request.SearchQuery) ||
                            wo.WoQTY.Contains(request.SearchQuery) ||
                            wo.WoStatus.Contains(request.SearchQuery) ||
                            wo.User.DisplayName.Contains(request.SearchQuery)
                        );
                        query = query.OrderByDescending(wo => wo.WoCreate);
                    }
                    else if (request.Category == "All" && string.IsNullOrEmpty(request.SearchQuery))
                    {
                        query = query.OrderByDescending(wo => wo.WoCreate);
                    }
                    else
                    {
                        switch (request.Category)
                        {
                            case "WoNumber":
                                query = query.Where(wo => wo.WoNumber.Contains(request.SearchQuery));
                                break;
                            case "SONumber":
                                query = query.Where(wo => wo.SONumber.Contains(request.SearchQuery));
                                break;
                            case "WoReferenceID":
                                query = query.Where(wo => wo.WoReferenceID.Contains(request.SearchQuery));
                                break;
                            case "WoQTY":
                                query = query.Where(wo => wo.WoQTY.Contains(request.SearchQuery));
                                break;
                            case "WoStatus":
                                query = query.Where(wo => wo.WoStatus.Contains(request.SearchQuery));
                                break;
                            case "DisplayName":
                                query = query.Where(wo => wo.User.DisplayName.Contains(request.SearchQuery));
                                break;
                            default:
                                // Handle unsupported category here
                                break;
                        }
                        query = query.OrderByDescending(wo => wo.WoCreate);
                    }
                }
                else
                {
                    query = query.OrderByDescending(wo => wo.WoCreate);
                }
               
                

                var totalItems = await query.CountAsync();
                var currentPage = request.PageNumber;
                var pageSize = request.PageSize;

                var workOrders = await query
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var workOrderDtos = _mapper.Map<List<WorkOrderDto>>(workOrders);

                return new ListResult<WorkOrderDto>
                {
                    Items = workOrderDtos,
                    TotalItems = totalItems,
                    CurrentPage = currentPage,
                    PageSize = pageSize
                };
            }
        }
    }
}
