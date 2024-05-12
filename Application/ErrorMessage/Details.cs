using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ErrorMessage
{
    public class Details
    {
        public class Query : IRequest<Domain.Model.ErrorMessage>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Domain.Model.ErrorMessage>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<bool> IsUnique(Guid id)
            {
                return await _context.ErrorMessages.AnyAsync(u => u.Id == id);
            }

            public async Task<Domain.Model.ErrorMessage> Handle(Query request, CancellationToken cancellationToken)
            {
                if (!await IsUnique(request.Id))
                {
                    throw new Exception("Id :'" + request.Id + "' Tidak ditemukan.");
                }

                return await _context.ErrorMessages.FirstOrDefaultAsync(u => u.Id == request.Id);
            }
        }
    }
}