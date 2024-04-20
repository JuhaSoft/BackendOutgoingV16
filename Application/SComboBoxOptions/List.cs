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
    public class List
    {
        public class CBOQuery : IRequest<List<SComboBoxOption>> {
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
         }
        public class Handler : IRequestHandler<CBOQuery, List<SComboBoxOption>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context , IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }
            public async Task<List<SComboBoxOption>> Handle(CBOQuery request, CancellationToken cancellationToken)
            {
                if (request.PageNumber <= 0)
                {
                return await _context.SComboBoxOptions.Select(SComboBoxOption => _mapper.Map<SComboBoxOption>(SComboBoxOption))
                .ToListAsync();
                   
                }
                else
                {
                
                     return await _context.SComboBoxOptions.Select(SComboBoxOption => _mapper.Map<SComboBoxOption>(SComboBoxOption))
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .ToListAsync();
                }
                // return await _context.SComboBoxOptions.ToListAsync();

                //throw new NotImplementedException();
            }

            
        }
    }
}