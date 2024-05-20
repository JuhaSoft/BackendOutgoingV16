using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.core;
using AutoMapper;
using Common.DTOs;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataReferences
{
    public class Detail
    {
       public class Query : IRequest<DataReferenceDTO>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, DataReferenceDTO>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                this._context = context;
            }
            public async Task<bool> IsUnique(Guid Id)
            {
                return await _context.DataReferences.AnyAsync(u => u.Id == Id);
            }
            public async Task<DataReferenceDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                if (!await IsUnique(request.Id))
                {
                    throw new Exception("Id  :'" + request.Id + "' Tidak ditemukan.");
                }

                var dataReference = await _context.DataReferences
                       .Include(dt => dt.LastStationID)
                       .ThenInclude(dt => dt.DataLine)
                       .Include(dt => dt.DataReferenceParameterChecks)
                       .ThenInclude(drpc => drpc.ParameterCheck)
                       .ThenInclude(pc => pc.ParameterCheckErrorMessages)
                       .ThenInclude(pcem => pcem.ErrorMessage)
                       .FirstOrDefaultAsync(u => u.Id == request.Id);

                if (dataReference == null)
                {
                    throw new Exception("Id :'" + request.Id + "' Tidak ditemukan.");
                }

                // Step 1: Get list of ParameterCheckIds
                var parameterCheckIds = dataReference.DataReferenceParameterChecks
                    .Select(drpc => drpc.ParameterCheckId)
                    .ToList();

                // Step 2: Get top errors in the last 6 months
                var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);
                var topErrors = await _context.ErrorTrack
                    .Where(et => parameterCheckIds.Contains(et.PCID) && et.TrackingDateCreate >= sixMonthsAgo)
                    .GroupBy(et => new { et.ErrorMessage.ErrorCode, et.ErrorMessage.ErrorDescription })
                    .Select(group => new ErrorMessageDatatrackDTO
                    {
                        ErrorCode = group.Key.ErrorCode,
                        ErrorDescription = group.Key.ErrorDescription,
                        Count = group.Count()
                    })
                    .OrderByDescending(x => x.Count)
                    .Take(5)
                    .ToListAsync();

                // Step 3: Map dataReference to DataReferenceDTO and add topErrors
                var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>()));
                var dataReferenceDetailDTO = mapper.Map<DataReferenceDTO>(dataReference);
                dataReferenceDetailDTO.TopErrors = topErrors;

                return dataReferenceDetailDTO;
            }

        }
    }
}