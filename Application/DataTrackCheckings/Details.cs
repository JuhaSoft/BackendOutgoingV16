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

namespace Application.DataTrackCheckings
{
    public class Details
    {
        public class Query : IRequest<DataTrackChecking>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, DataTrackChecking>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                this._context = context;
            }
            public async Task<bool> IsiDUnique(Guid Id)
            {
                return await _context.DataTrackCheckings.AnyAsync(u => u.Id == Id);
            }
            public async Task<DataTrackChecking> Handle(Query request, CancellationToken cancellationToken)
            {
                
                if (!await IsiDUnique(request.Id))
                {
                    
                    throw new Exception("Id  :'" + request.Id + "' Tidak ditemukan.");
                }
                return await _context.DataTrackCheckings
                    .Include(dt => dt.ParameterChecks)
                    .FirstOrDefaultAsync(u => u.Id == request.Id);
            }

           
        }
    }
}