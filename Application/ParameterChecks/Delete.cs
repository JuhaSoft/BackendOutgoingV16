using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;


namespace Application.ParameterChecks
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

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var parameterChecks = await _context.ParameterChecks
                    .Where(pc => pc.DataReferenceId == request.Id)
                                       .ToListAsync(cancellationToken);

                if (parameterChecks == null || !parameterChecks.Any())
                {
                    throw new Exception("ParameterChecks tidak ditemukan.");
                }

                _context.ParameterChecks.RemoveRange(parameterChecks);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
