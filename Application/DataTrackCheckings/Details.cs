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

                var mappedDataTrackCheckings = new List<DataTrackCheckingDTO>();

                foreach (var dataTrackChecking in dataTrackCheckings)
                {
                    var mappedDataTrackChecking = _mapper.Map<DataTrackCheckingDTO>(dataTrackChecking);

                    // Ambil ParameterCheck
                    var parameterCheck = await _context.ParameterChecks
                        .Include(pc => pc.DataReferenceParameterChecks)
                        .Include(pc => pc.ParameterCheckErrorMessages)
                        .FirstOrDefaultAsync(pc => pc.Id == mappedDataTrackChecking.PCID);

                    mappedDataTrackChecking.ParameterCheck = _mapper.Map<ParameterCheckDTO>(parameterCheck);

                    // Ambil ErrorMessage jika ada
                    if (mappedDataTrackChecking.ErrorMessage != null)
                    {
                        var errorMessage = await _context.ErrorMessages.FindAsync(mappedDataTrackChecking.ErrorMessage.Id);
                        mappedDataTrackChecking.ErrorMessage = _mapper.Map<ErrorMessageDTO>(errorMessage);
                    }

                    // Ambil ErrorTracks
                    await AddErrorTracks(mappedDataTrackChecking);

                    mappedDataTrackCheckings.Add(mappedDataTrackChecking);
                }

                return mappedDataTrackCheckings;
            }

            private async Task AddErrorTracks(DataTrackCheckingDTO mappedDataTrackChecking)
            {
                var sixMonthsAgo = DateTime.Today.AddMonths(-6);

                var errorTracks = await _context.ErrorTrack
                    .Where(et => et.PCID == mappedDataTrackChecking.PCID && et.TrackingDateCreate >= sixMonthsAgo)
                    .GroupBy(et => new { et.ErrorId, et.ErrorMessage.ErrorCode, et.ErrorMessage.ErrorDescription })
                    .Select(group => new
                    {
                        ErrorId = group.Key.ErrorId,
                        ErrorCode = group.Key.ErrorCode,
                        ErrorDescription = group.Key.ErrorDescription,
                        Count = group.Count() // Menggunakan group.Count() untuk mendapatkan jumlahnya
                    })
                    .OrderByDescending(group => group.Count)
                    .Take(5)
                    .ToListAsync();

                // Konversi ke DTO
                var errorTrackDTOs = errorTracks.Select(et => new ErrorMessageDatatrackDTO
                {
                    ErrorId = et.ErrorId,
                    ErrorCode = et.ErrorCode,
                    ErrorDescription = et.ErrorDescription,
                    Count = et.Count
                }).ToList();

                // Tambahkan ErrorTracks ke dalam DataTrackCheckingDTO
                mappedDataTrackChecking.ErrorTracks = errorTrackDTOs;
            }
        }
    }
}
