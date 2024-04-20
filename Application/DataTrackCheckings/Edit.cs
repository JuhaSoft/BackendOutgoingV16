using AutoMapper;
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
    public class Edit
    {
        public class Command : IRequest {
        public DataTrackChecking DataTrackChecking { get; set; }
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

            public async Task<bool> IsDTCUnique(Guid id)
            {
                return await _context.DataTrackCheckings.AnyAsync(u => u.Id == id);
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
               
                if (!await IsDTCUnique(request.DataTrackChecking.Id))

                {
                    // Username sudah ada, Anda dapat menangani kasus ini sesuai kebutuhan aplikasi Anda.
                    // Misalnya, lempar pengecualian, kirimkan respons yang sesuai, dll.
                    throw new Exception("ID " + request.DataTrackChecking.Id + "' Tidak ditemukan.");
                }
                string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
                var datatrackChecking = await _context.DataTracks.FirstOrDefaultAsync(u => u.Id == request.DataTrackChecking.Id);
                _mapper.Map(request.DataTrackChecking,datatrackChecking);
            await _context.SaveChangesAsync();
            return Unit.Value;
            }

        }
    }
}