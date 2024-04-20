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
    public class Detail
    {
        public class Query : IRequest<SComboBoxOption>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, SComboBoxOption>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }
            public async Task<bool> IsCBOUnique(Guid id)
            {
                return await _context.SComboBoxOptions.AnyAsync(u => u.Id == id);
            }
            public async Task<SComboBoxOption> Handle(Query request, CancellationToken cancellationToken)
            {
                
                if (!await IsCBOUnique(request.Id))
                {
                    
                    throw new Exception("Id :'" + request.Id + "' Tidak ditemukan.");
                }

                var scombo = await _context.SComboBoxOptions.FirstOrDefaultAsync(u => u.Id == request.Id);
                if (scombo == null)
                {
                    throw new Exception("UserId '" + request.Id + "' tidak ditemukan.");
                }
                //return await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId);
                return _mapper.Map<SComboBoxOption>(scombo);
                 //return await _context.SComboBoxOptions.FirstOrDefaultAsync(u => u.Id == request.Id);
            }

          
        }
    }
}