using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DataTracks
{
    public class Edit
    {
        public class Command : IRequest {
        public DataTrack DataTrack { get; set; }
        }
  
        public class Handler : IRequestHandler<Command>
        {
        private readonly IMapper _mapper;
 
        private readonly DataContext _context;
            public Handler(DataContext context, IMapper mapper)
            {
            this._mapper = mapper;
            this._context = context;
            }

            public async Task<bool> IsPSNUnique(Guid id)
            {
                return await _context.DataTracks.AnyAsync(u => u.Id == id);
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
               
                if (!await IsPSNUnique(request.DataTrack.Id))

                {
                    // Username sudah ada, Anda dapat menangani kasus ini sesuai kebutuhan aplikasi Anda.
                    // Misalnya, lempar pengecualian, kirimkan respons yang sesuai, dll.
                    throw new Exception("ID Untuk PSN" + request.DataTrack.TrackPSN + "' Tidak ditemukan.");
                }
                string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
                var datatrack = await _context.DataTracks.FirstOrDefaultAsync(u => u.Id == request.DataTrack.Id);
                _mapper.Map(request.DataTrack,datatrack);
            await _context.SaveChangesAsync();
            return Unit.Value;
            }

        }
    }
}