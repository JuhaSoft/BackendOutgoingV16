using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ErrorMessage
{
    public class List
    {
        public class Query : IRequest<ListResult<Domain.Model.ErrorMessage>>
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public string Category { get; set; }
            public string SearchQuery { get; set; }
        }

        public class ListResult<T>
        {
            public List<T> Items { get; set; }
            public int TotalItems { get; set; }
            public int TotalPages => (int)Math.Ceiling((double)TotalItems / (double)PageSize);
            public int CurrentPage { get; set; }
            public int PageSize { get; set; }
        }

        public class Handler : IRequestHandler<Query, ListResult<Domain.Model.ErrorMessage>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ListResult<Domain.Model.ErrorMessage>> Handle(Query request, CancellationToken cancellationToken)
            {
                IQueryable<Domain.Model.ErrorMessage> query = _context.ErrorMessages;

                if (!string.IsNullOrEmpty(request.SearchQuery))
                {
                    if (request.Category == "All" && !string.IsNullOrEmpty(request.SearchQuery))
                    {
                        query = query.Where(dl =>
                            dl.ErrorCode.Contains(request.SearchQuery) ||
                            dl.ErrorDescription.Contains(request.SearchQuery)
                        );
                        query = query.OrderByDescending(dt => dt.ErrorCode);
                    }
                    else if (request.Category == "All" && string.IsNullOrEmpty(request.SearchQuery))
                    {
                        query = query.OrderByDescending(dt => dt.ErrorCode);
                    }
                    else
                    {
                        switch (request.Category)
                        {
                            case "ErrorCode":
                                query = query.Where(dt => dt.ErrorCode.Contains(request.SearchQuery));
                                break;
                            case "ErrorDescription":
                                query = query.Where(dt => dt.ErrorDescription.Contains(request.SearchQuery));
                                break;
                            default:
                                break;
                        }
                        query = query.OrderByDescending(dt => dt.ErrorCode);
                    }
                }
                else
                {
                    query = query.OrderByDescending(dt => dt.ErrorCode);
                }

                var totalItems = await query.CountAsync();
                var currentPage = request.PageNumber;
                var pageSize = request.PageSize;

                var dataLine = await query
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var errorMessages = _mapper.Map<List<Domain.Model.ErrorMessage>>(dataLine);

                return new ListResult<Domain.Model.ErrorMessage>
                {
                    Items = errorMessages,
                    TotalItems = totalItems,
                    CurrentPage = currentPage,
                    PageSize = pageSize
                };
            }
        }
    }
}