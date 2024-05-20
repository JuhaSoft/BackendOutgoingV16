using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.DTOs;
using Common.DTOs.DataTrack;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataTracks
{
    public class ListChart
    {
        public class DtQuery : IRequest<DtListResult>
        {
            public string Start { get; set; }
            public string EndDate { get; set; }
        }

        public class DtListResult
        {
            public List<DataTrackGrapDTO> DataTracks { get; set; }
        }

        public class Handler : IRequestHandler<DtQuery, DtListResult>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<DtListResult> Handle(DtQuery request, CancellationToken cancellationToken)
            {
                IQueryable<DataTrack> query = _context.DataTracks
                    .Where(dt =>
                        !dt.DTisDeleted &&
                        dt.DataTrackCheckings.All(dtc => !dtc.DTCisDeleted));

                if (!string.IsNullOrEmpty(request.Start) && DateTime.TryParse(request.Start, out DateTime startDate))
                {
                    query = query.Where(dt => dt.TrackingDateCreate >= startDate);
                }

                if (!string.IsNullOrEmpty(request.EndDate) && DateTime.TryParse(request.EndDate, out DateTime endDate))
                {
                    query = query.Where(dt => dt.TrackingDateCreate <= endDate);
                }

                query = query.OrderByDescending(dt => dt.TrackingDateCreate);

                var dataTracks = await query.ToListAsync(cancellationToken);

                var result = new DtListResult
                {
                    DataTracks = _mapper.Map<List<DataTrackGrapDTO>>(dataTracks)
                };

                return result;
            }
        }
    }
}
