using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ParameterChecks
{
    public class Create
    {
        public class Command : IRequest<Unit>
        {
            public Guid DataReferenceId { get; set; }
            public List<ParameterCheckDto> ParameterChecks { get; set; }
        }

        public class ParameterCheckDto
        {
            public string Description { get; set; }
            public int Order { get; set; }
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
                // Periksa apakah DataReferenceId ada di dalam DataReference
                var dataReference = await _context.DataReferences.FindAsync(request.DataReferenceId);

                if (dataReference == null)
                {
                    throw new Exception("DataReferenceId tidak ditemukan.");
                }

                // Periksa apakah ParameterChecks sudah ada untuk DataReferenceId yang sama
                var existingParameterChecks = await _context.ParameterChecks
                    .Where(pc => pc.DataReferenceId == request.DataReferenceId)
                    .ToListAsync();

                if (existingParameterChecks.Any())
                {
                    throw new Exception("ParameterChecks untuk Reference yang sama sudah ada.");
                }

                // Jika tidak ada ParameterChecks yang sudah ada, buat entri baru
                foreach (var parameterCheckDto in request.ParameterChecks)
                {
                    var parameterCheck = new ParameterCheck
                    {
                        Id = Guid.NewGuid(), // Buat Id baru
                        Description = parameterCheckDto.Description,
                        Order = parameterCheckDto.Order,
                        DataReferenceId = request.DataReferenceId
                    };

                    _context.ParameterChecks.Add(parameterCheck);
                }

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
