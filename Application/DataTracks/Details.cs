using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.DTOs;
using Common.DTOs.DataTrack;

namespace Application.DataTracks
{
    public class Details
    {
        public class Query : IRequest<DetailDataTrackDto>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, DetailDataTrackDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<DetailDataTrackDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var dataTrack = await _context.DataTracks
                    .Include(dt => dt.User)
                    .Include(dt => dt.LastStationID)
                    .ThenInclude(dt => dt.DataLine)
                    .Include(dt => dt.DataTrackCheckings)
                    .ThenInclude(dt => dt.ParameterCheck)
                    .Include(dt => dt.DataTrackCheckings)
                    .ThenInclude(dt => dt.ImageDataChecks)
                    .Where(dt => !dt.DTisDeleted && dt.DataTrackCheckings.All(dtc => !dtc.DTCisDeleted))
                    .FirstOrDefaultAsync(u => u.Id == request.Id);

                if (dataTrack == null)
                {
                    throw new Exception("PSN :'" + request.Id + "' Tidak ditemukan.");
                }

                return _mapper.Map<DetailDataTrackDto>(dataTrack);
            }
        }
    }
}
