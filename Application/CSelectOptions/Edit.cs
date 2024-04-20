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

namespace Application.CSelectOptions
{
    public class Edit
    {
        public class Command : IRequest 
        {
            public SelectOption   SelectOption { get; set; }
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
                return await _context.SelectOptions.AnyAsync(u => u.Id == id);
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
               
                if (!await IsUnique(request.SelectOption.Id))

                {
                    // Username sudah ada, Anda dapat menangani kasus ini sesuai kebutuhan aplikasi Anda.
                    // Misalnya, lempar pengecualian, kirimkan respons yang sesuai, dll.
                    throw new Exception("ID Untuk Options" + request.SelectOption.Id + "' Tidak ditemukan.");
                }
        
                var dataSelectOption = await _context.SelectOptions.FirstOrDefaultAsync(u => u.Id == request.SelectOption.Id);
                _mapper.Map(request.SelectOption,dataSelectOption);
            await _context.SaveChangesAsync();
            return Unit.Value;
            }

            
        }
    }
}