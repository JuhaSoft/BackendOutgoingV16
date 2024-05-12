using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Domain.Model;
using Persistence;
using System.Threading;

namespace Application.ErrorMessage
{
    public class Create
    {
        public class Command : IRequest {
            public Domain.Model.ErrorMessage  ErrorMessage { get; set; }
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
                if (_context.ErrorMessages.Any(dl => dl.ErrorCode == request.ErrorMessage.ErrorCode))
                {
                    throw new Exception("Error Code sudah terdaftar.");
                }
                _context.ErrorMessages.Add(request.ErrorMessage);
                await _context.SaveChangesAsync();
                return Unit.Value;
            }

           
        }

    }
}