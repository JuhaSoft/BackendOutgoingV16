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

namespace Application.DataTracks
{
    public class List
    {
        public class DtQuery : IRequest<DtListResult>
        {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public string Category { get; set; } // Tambahkan properti Category
            public string SearchQuery { get; set; } // Tambahkan properti SearchQuery
            public string Start { get; set; }
            public string EndDate { get; set; }
        }

        public class DtListResult
        {
            public List<DataTrackDTO> DataTracks { get; set; }
            public int TotalItems { get; set; }
            public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
            public int CurrentPage { get; set; }
            public int PageSize { get; set; }
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
                var result = new DtListResult
                {
                    CurrentPage = request.PageNumber,
                    PageSize = request.PageSize
                };
                var abc = request.Category;
                var abcd = request.SearchQuery;
                IQueryable<DataTrack> query = _context.DataTracks
                    .Include(dt => dt.User)
                    .Include(dt => dt.LastStationID)
                    .ThenInclude(dt => dt.DataLine)
                    .Include(dt => dt.DataTrackCheckings)
                    .ThenInclude(dt => dt.ParameterCheck)
                    .Include(dt => dt.DataTrackCheckings)
                    .ThenInclude(dtck => dtck.ErrorMessage)
                     .Include(dt => dt.DataTrackCheckings)
                    .ThenInclude(dt => dt.ImageDataChecks)
                   .Where(dt =>
                                !dt.DTisDeleted &&
                                dt.DataTrackCheckings.All(dtc => !dtc.DTCisDeleted) &&
                                (
                                    string.IsNullOrEmpty(request.SearchQuery) ||
                                    (
                                    request.Category == "All" &&
                                    (
                                    dt.User.DisplayName.Contains(request.SearchQuery) ||
                                    dt.TrackPSN.Contains(request.SearchQuery) ||
                                    dt.TrackReference.Contains(request.SearchQuery) ||
                                    dt.TrackingWO.Contains(request.SearchQuery) ||
                                    dt.TrackingResult.Contains(request.SearchQuery) ||
                                    dt.TrackingDateCreate.ToString().Contains(request.SearchQuery)
                                    )
                                ) ||
                                (
                                    request.Category == "TrackPSN" && dt.TrackPSN.Contains(request.SearchQuery)
                                ) ||
                                (
                                request.Category == "DisplayName" && dt.User.DisplayName.Contains(request.SearchQuery)
                                 ) ||
                                (
                                    request.Category == "TrackReference" && dt.TrackReference.Contains(request.SearchQuery)
                                ) ||
                                (
                                    request.Category == "TrackingWO" && dt.TrackingWO.Contains(request.SearchQuery)
                                ) ||
                                (
                                    request.Category == "TrackingResult" && dt.TrackingResult.Contains(request.SearchQuery)
                                ) ||
                                (
                                    request.Category == "TrackingDateCreate" && dt.TrackingDateCreate.ToString().Contains(request.SearchQuery)
                                )
                                ) &&
        (
            string.IsNullOrEmpty(request.Start) || dt.TrackingDateCreate >= DateTime.Parse(request.Start)
        ) &&
        (
            string.IsNullOrEmpty(request.EndDate) || dt.TrackingDateCreate <= DateTime.Parse(request.EndDate)
        )
                    );

                query = query.OrderByDescending(dtt => dtt.TrackingDateCreate);

                result.TotalItems = await query.CountAsync();

                if (request.PageNumber <= 0)
                {
                    result.DataTracks = await query
                        .Take(request.PageSize)
                        .Select(dt => _mapper.Map<DataTrackDTO>(dt))
                        .ToListAsync();
                }
                else
                {
                    result.DataTracks = await query
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .Select(dt => _mapper.Map<DataTrackDTO>(dt))
                        .ToListAsync();
                }

                return result;
            }
        }
    }
}
