using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Model;
using MediatR;
using Persistence;

namespace Application.DataLines
{
    public class Create
    {
        public class Command : IRequest {
            public DataLine DataLine { get; set; }
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
                if (_context.DataLines.Any(dl => dl.LineId == request.DataLine.LineId))
                {
                    throw new Exception("Line Id sudah terdaftar.");
                }
                _context.DataLines.Add(request.DataLine);
                await _context.SaveChangesAsync();
                return Unit.Value;
            }

           
        }

    }
}