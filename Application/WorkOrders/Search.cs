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

namespace Application.WorkOrders
{
    public class Search
    {
        public class Query : IRequest<ListResult<WorkOrderDto>>
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public string SearchTerm { get; set; } // Tambahkan properti SearchTerm
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

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    // Lakukan pencarian di semua field yang relevan
                    query = query.Where(wo =>
                        wo.WoNumber.Contains(request.SearchTerm) ||
                        wo.SONumber.Contains(request.SearchTerm) ||
                        wo.WoReferenceID.Contains(request.SearchTerm) ||
                        // tambahkan kondisi untuk field lainnya sesuai kebutuhan
                        // ...
                        wo.User.DisplayName.Contains(request.SearchTerm) ||
                        wo.User.UserName.Contains(request.SearchTerm)
                    );
                }

                query = query.OrderByDescending(wo => wo.WoNumber);

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