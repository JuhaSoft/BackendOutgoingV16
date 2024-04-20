using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.core;
using AutoMapper;
using Common.DTOs;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.WorkOrders
{
    public class Details
    {
        public class Query : IRequest<WorkOrderDto>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, WorkOrderDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }
            public async Task<bool> IsUnique(Guid id)
            {
                return await _context.WorkOrders.AnyAsync(u => u.Id == id);
            }
            public async Task<WorkOrderDto> Handle(Query request, CancellationToken cancellationToken)
            {

                if (!await IsUnique(request.Id))
                {

                    throw new Exception("Wo '" + request.Id + "' Tidak ditemukan.");
                }

                var workorder = await _context.WorkOrders
                            .Where(u => u.Id == request.Id)
                            .Include(wo => wo.User)
                            .FirstOrDefaultAsync(cancellationToken);
                if (workorder == null)
                {
                    throw new Exception("WO  '" + request.Id + "' tidak ditemukan.");
                }
                //return await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId);
                return _mapper.Map<WorkOrderDto>(workorder);


            }

            
        }
    }
}