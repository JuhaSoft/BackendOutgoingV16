using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ErrorMessage
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
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

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var errorMessage = await _context.ErrorMessages.FindAsync(request.Id);

                if (errorMessage == null)
                {
                    throw new Exception("Data Tidak ditemukan.");
                }

                _context.ErrorMessages.Remove(errorMessage);
                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}