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

namespace Application.LastStationIDs
{
    public class Edit
    {
        public class Command : IRequest {
        public LastStationID LastStationID { get; set; }
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

            public async Task<bool> IsUnique(Guid id)
            {
                return await _context.LastStationIDs.AnyAsync(u => u.Id == id);
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var lastStationID = await _context.LastStationIDs.FirstOrDefaultAsync(u => u.Id == request.LastStationID.Id);

                if (lastStationID == null)
                {
                    throw new Exception("ID " + request.LastStationID.Id + " tidak ditemukan.");
                }

                // Update properti satu per satu
                lastStationID.StationID = request.LastStationID.StationID;
                lastStationID.StationName = request.LastStationID.StationName;
                lastStationID.LineId = request.LastStationID.LineId;

                // Simpan perubahan ke dalam konteks
                await _context.SaveChangesAsync();

                return Unit.Value;
            }




        }
    }
}