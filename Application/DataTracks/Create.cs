using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Model;
using MediatR;
using Persistence;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
namespace Application.DataTracks
{
    public class Create
    {
        public class Command : IRequest {
            public DataTrack DataTrack { get; set; }
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
             
                _context.DataTracks.Add(request.DataTrack);
                await _context.SaveChangesAsync();
                return Unit.Value;
            }
        }
 
    }
}