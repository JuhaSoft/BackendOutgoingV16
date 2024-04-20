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

namespace Application.DataLines
{
    public class Edit
    {
        public class Command : IRequest {
        public DataLine dataLine { get; set; }
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
                return await _context.DataLines.AnyAsync(u => u.Id == id);
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
               
                if (!await IsUnique(request.dataLine.Id))

                {
                    // Username sudah ada, Anda dapat menangani kasus ini sesuai kebutuhan aplikasi Anda.
                    // Misalnya, lempar pengecualian, kirimkan respons yang sesuai, dll.
                    throw new Exception("ID " + request.dataLine.Id + "' Tidak ditemukan.");
                }
                var dataLineCheck = await _context.DataLines.FirstOrDefaultAsync(u => u.Id == request.dataLine.Id);

                if (dataLineCheck != null)
                {
                    dataLineCheck.LineId = request.dataLine.LineId; // Ganti Property1 dengan properti yang ingin Anda perbarui
                    dataLineCheck.LineName = request.dataLine.LineName; // Ganti Property2 dengan properti yang ingin Anda perbarui
                                                                          // Lakukan ini untuk semua properti yang ingin Anda perbarui

                    await _context.SaveChangesAsync();
                }
            //    _mapper.Map(request.dataLine, request.dataLine);
            //await _context.SaveChangesAsync();
            return Unit.Value;
            }

        }
    }
}