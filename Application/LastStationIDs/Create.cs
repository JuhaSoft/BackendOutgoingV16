using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.LastStationIDs
{
    public class Create
    {
        public class Command : IRequest {
            public LastStationID LastStationID { get; set; }
        }
  
        public class Handler : IRequestHandler<Command>
        {
 
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }


            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                // Cari apakah LineId sudah terdaftar
                if (!_context.DataLines.Any(dl => dl.Id == request.LastStationID.LineId))
                {
                    throw new Exception("Id Tidak terdaftar.");
                }

                // Buat objek LastStationID baru
                var newLastStationID = new LastStationID
                {
                    Id = Guid.NewGuid(), // Atau gunakan id yang diberikan oleh klien jika Anda ingin itu
                    StationID = request.LastStationID.StationID,
                    StationName = request.LastStationID.StationName,
                    LineId = request.LastStationID.LineId
                };

                // Tambahkan objek baru ke dalam konteks
                _context.LastStationIDs.Add(newLastStationID);

                // Simpan perubahan ke dalam database
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    // Tangani exception di sini
                    Console.WriteLine(ex.InnerException.Message);
                    throw;
                }

                return Unit.Value;
            }


        }

    }
}