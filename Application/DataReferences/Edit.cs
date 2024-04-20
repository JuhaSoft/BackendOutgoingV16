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

namespace Application.DataReferences
{
    public class Edit
    {
        public class Command : IRequest {
        public DataReference dataReference { get; set; }
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
                return await _context.DataReferences.AnyAsync(u => u.Id == id);
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
               
                if (!await IsUnique(request.dataReference.Id))

                {
                    // Username sudah ada, Anda dapat menangani kasus ini sesuai kebutuhan aplikasi Anda.
                    // Misalnya, lempar pengecualian, kirimkan respons yang sesuai, dll.
                    throw new Exception("ID " + request.dataReference.Id + "' Tidak ditemukan.");
                }
                var dataRefCheck = await _context.DataReferences.FirstOrDefaultAsync(u => u.Id == request.dataReference.Id);

                if (dataRefCheck != null)
                {
                    dataRefCheck.RefereceName = request.dataReference.RefereceName; // Ganti Property1 dengan properti yang ingin Anda perbarui
                    dataRefCheck.StationID = request.dataReference.StationID; // Ganti Property2 dengan properti yang ingin Anda perbarui
                     dataRefCheck.PsnPos = request.dataReference.PsnPos; // Ganti Property1 dengan properti yang ingin Anda perbarui
                    dataRefCheck.RefPos = request.dataReference.RefPos; 
                     dataRefCheck.RefCompare = request.dataReference.RefCompare; // Ganti Property1 dengan properti yang ingin Anda perbarui
                    await _context.SaveChangesAsync();
                }
            //    _mapper.Map(request.dataLine, request.dataLine);
            //await _context.SaveChangesAsync();
            return Unit.Value;
            }

        }
    }
}