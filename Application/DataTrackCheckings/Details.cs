using AutoMapper;
using Common.DTOs;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DataTrackCheckings
{
    public class Details
    {
        public class Query : IRequest<IEnumerable<DataTrackCheckingDTO>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<DataTrackCheckingDTO>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<bool> IsiDUnique(Guid Id)
            {
                return await _context.DataTrackCheckings.AnyAsync(u => u.DataTrackID == Id);
            }

            public async Task<IEnumerable<DataTrackCheckingDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (!await IsiDUnique(request.Id))
                {
                    throw new Exception("Id :'" + request.Id + "' Tidak ditemukan.");
                }

                var dataTrackCheckings = await _context.DataTrackCheckings
                    .Where(dtc => dtc.DataTrackID == request.Id)
                    .Include(dtc => dtc.ParameterCheck)
                        .ThenInclude(pc => pc.DataReferenceParameterChecks)
                    .Include(dtc => dtc.ImageDataChecks)
                    .ToListAsync();

                var mappedDataTrackCheckings = _mapper.Map<IEnumerable<DataTrackCheckingDTO>>(dataTrackCheckings);

                foreach (var dto in mappedDataTrackCheckings)
                {
                    var parameterCheck = await _context.ParameterChecks
                        .Include(pc => pc.DataReferenceParameterChecks)
                        .FirstOrDefaultAsync(pc => pc.Id == dto.PCID);

                    dto.ParameterCheck = _mapper.Map<ParameterCheckDTO>(parameterCheck);
                }

                return mappedDataTrackCheckings;
            }
            //  public async Task<IEnumerable<DataTrackCheckingDTO>> Handle(Query request, CancellationToken cancellationToken)
            // {
            //     if (!await IsiDUnique(request.Id))
            //     {
            //         throw new Exception("Id :'" + request.Id + "' Tidak ditemukan.");
            //     }

            //     var dataTrackCheckings = await _context.DataTrackCheckings
            //         .Where(dtc => dtc.DataTrackID == request.Id)
            //         .Include(dtc => dtc.ParameterCheck)
            //         .Include(dtc => dtc.ImageDataChecks)
            //         .ToListAsync();

            //     return _mapper.Map<IEnumerable<DataTrackCheckingDTO>>(dataTrackCheckings);
            // }
        }
    }
}