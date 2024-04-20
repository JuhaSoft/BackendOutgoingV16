using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domain.Model;


using Persistence;
using Microsoft.EntityFrameworkCore;
namespace Application.DataContrplTypes
{
    public class Detail
    {
         public class Query : IRequest<DataContrplType>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, DataContrplType>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                this._context = context;
            }
            public async Task<bool> IsCTUnique(Guid id)
            {
                return await _context.DataContrplTypes.AnyAsync(u => u.Id == id);
            }
            public async Task<DataContrplType> Handle(Query request, CancellationToken cancellationToken)
            {
                
                if (!await IsCTUnique(request.Id))
                {
                    
                    throw new Exception("Id :'" + request.Id + "' Tidak ditemukan.");
                }
                return await _context.DataContrplTypes.FirstOrDefaultAsync(u => u.Id == request.Id);
            }

            
        }
    }
}