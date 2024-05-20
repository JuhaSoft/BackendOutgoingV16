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
namespace Application.ErrorTracks
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
            public List<ErrorTrackChartDTO> DataTracks { get; set; }
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
                // Mulai dengan semua data dari tabel ErrorTrack
                IQueryable<ErrorTrack> query = _context.ErrorTrack
                    .Include(et => et.ErrorMessage)
                    .AsQueryable();

                // Tambahkan kondisi jika Start date ada dan valid
                if (!string.IsNullOrEmpty(request.Start) && DateTime.TryParse(request.Start, out DateTime startDate))
                {
                    query = query.Where(dt => dt.TrackingDateCreate >= startDate);
                }

                // Tambahkan kondisi jika End date ada dan valid
                if (!string.IsNullOrEmpty(request.EndDate) && DateTime.TryParse(request.EndDate, out DateTime endDate))
                {
                    query = query.Where(dt => dt.TrackingDateCreate <= endDate);
                }

                // Urutkan hasil kueri
                query = query.OrderByDescending(dt => dt.TrackingDateCreate);

                // Eksekusi kueri dan ambil hasilnya
                var dataTracks = await query.ToListAsync(cancellationToken);

                // Peta hasil ke DTO
                var result = new DtListResult
                {
                    DataTracks = _mapper.Map<List<ErrorTrackChartDTO>>(dataTracks)
                };

                // Kembalikan hasil
                return result;
            }

        }
    }
}