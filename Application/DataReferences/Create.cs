using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Model;
using MediatR;
using Persistence;

namespace Application.DataReferences
{
    public class Create
    {
        public class Command : IRequest {
            public DataReference DataReference { get; set; }
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
                if (_context.DataReferences.Any(dl => dl.RefereceName == request.DataReference.RefereceName))
                {
                    throw new Exception("Line Id sudah terdaftar.");
                }
                _context.DataReferences.Add(request.DataReference);
                await _context.SaveChangesAsync();
                return Unit.Value;
            }

           
        }

    }
}