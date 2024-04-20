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

namespace Application.SComboBoxOptions
{
    public class Edit
    {
        public class Command : IRequest 
        {
            public SComboBoxOption   SComboBoxOption { get; set; }
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

            public async Task<bool> IsCTUnique(Guid id)
            {
                return await _context.SComboBoxOptions.AnyAsync(u => u.Id == id);
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
               
                if (!await IsCTUnique(request.SComboBoxOption.Id))

                {
                    // Username sudah ada, Anda dapat menangani kasus ini sesuai kebutuhan aplikasi Anda.
                    // Misalnya, lempar pengecualian, kirimkan respons yang sesuai, dll.
                    throw new Exception("ID Untuk Combo Option" + request.SComboBoxOption.Id + "' Tidak ditemukan.");
                }
        
                var comboBoxOption = await _context.SComboBoxOptions.FirstOrDefaultAsync(u => u.Id == request.SComboBoxOption.Id);
                _mapper.Map(request.SComboBoxOption,comboBoxOption);
            await _context.SaveChangesAsync();
            return Unit.Value;
            }

           
        }
    }
}