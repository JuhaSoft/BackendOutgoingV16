using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.DTOs;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ParameterChecks
{
    public class Edit
    {
        public class Command : IRequest<Unit>
        {
            public Guid DataReferenceId { get; set; }
            public List<ParameterCheckDto> ParameterChecks { get; set; }
        }

        public class ParameterCheckDto
        {
            public Guid Id { get; set; }
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
                foreach (var parameterCheckUpdate in request.ParameterChecks)
                {
                    var parameterCheck = await _context.ParameterChecks.FirstOrDefaultAsync(u => u.Id == parameterCheckUpdate.Id);

                    if (parameterCheck != null)
                    {
                        // Jika parameterCheck ditemukan, update deskripsi dan urutannya
                        parameterCheck.Description = parameterCheckUpdate.Description;
                        parameterCheck.Order = parameterCheckUpdate.Order;
                    }
                    else
                    {
                        // Jika tidak ditemukan, tambahkan parameterCheck baru
                        _context.ParameterChecks.Add(new ParameterCheck
                        {
                            Id = Guid.NewGuid(), // Generate ID baru
                            Description = parameterCheckUpdate.Description,
                            Order = parameterCheckUpdate.Order,
                            DataReferenceId = dataReference.Id
                        });
                    }
                }

                // Hapus parameterCheck yang tidak ada dalam request.ParameterChecks
                var existingParameterCheckIds = request.ParameterChecks.Select(pc => pc.Id);
                var parameterChecksToRemove = _context.ParameterChecks.Where(pc => pc.DataReferenceId == dataReference.Id && !existingParameterCheckIds.Contains(pc.Id));
                _context.ParameterChecks.RemoveRange(parameterChecksToRemove);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }

    }
}