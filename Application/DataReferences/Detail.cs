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
                //return await _context.DataReferences
                //     .Include(dt => dt.LastStationID)
                //     .ThenInclude(dt => dt.DataLine)
                //     .Include(dt => dt.DataReferenceParameterChecks)
                //     .ThenInclude(drpc => drpc.ParameterCheck)
                //     .ThenInclude(prc => prc.ParameterCheckErrorMessages)
                //    .FirstOrDefaultAsync(u => u.Id == request.Id);
                //return await _context.DataReferences
                //     .Include(dt => dt.LastStationID)
                //     .ThenInclude(dt => dt.DataLine)
                //     .Include(dt => dt.DataReferenceParameterChecks)
                //     .ThenInclude(drpc => drpc.ParameterCheck)
                //     .ThenInclude(pc => pc.ParameterCheckErrorMessages)
                //    .ThenInclude(pcem => pcem.ErrorMessage)
                //.Include(pc => pc.DataReferenceParameterChecks)
                //    .FirstOrDefaultAsync(u => u.Id == request.Id);

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

                var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>()));
                var dataReferenceDetailDTO = mapper.Map<DataReferenceDTO>(dataReference);

                return dataReferenceDetailDTO;
            }

            
        } 
    }
}