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
                        .ThenInclude(pc => pc.ParameterCheckErrorMessages)
                    .Include(dtc => dtc.ImageDataChecks)
                    .Include(dtc => dtc.ErrorMessage)
                    .Include(dtc => dtc.Approver)
                    .ToListAsync();

                var mappedDataTrackCheckings = dataTrackCheckings.Select(dtc => _mapper.Map<DataTrackCheckingDTO>(dtc)).ToList();
                //var mappedDataTrackCheckings = _mapper.Map<IEnumerable<DataTrackCheckingDTO>>(dataTrackCheckings);
                //var mappedDataTrackCheckings = dataTrackCheckings.ProjectTo<DataTrackCheckingDTO>(_mapper.ConfigurationProvider);
                foreach (var dto in mappedDataTrackCheckings)
                {
                    var parameterCheck = await _context.ParameterChecks
                        .Include(pc => pc.DataReferenceParameterChecks)
                        .Include(pc => pc.ParameterCheckErrorMessages)
                        .FirstOrDefaultAsync(pc => pc.Id == dto.PCID);

                    dto.ParameterCheck = _mapper.Map<ParameterCheckDTO>(parameterCheck);

                    if (dto.ErrorMessage != null)
                    {
                        var errorMessage = await _context.ErrorMessages.FindAsync(dto.ErrorMessage.Id);
                        dto.ErrorMessage = _mapper.Map<ErrorMessageDTO>(errorMessage);
                    }
                }

                return mappedDataTrackCheckings;


            }

        }
    }
}