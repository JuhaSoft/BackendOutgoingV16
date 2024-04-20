using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Model;
using MediatR;
using Persistence;

namespace Application.DataContrplTypes
{
    public class Create
    {
            public class Command : IRequest {
            public DataContrplType DataContrplType { get; set; }
        }
  
        public class Handler : IRequestHandler<Command>
        {
 
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

         
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
             
                _context.DataContrplTypes.Add(request.DataContrplType);
                await _context.SaveChangesAsync();
                return Unit.Value;
            }

            
        }
    }
}