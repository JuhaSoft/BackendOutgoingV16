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

namespace Application.WebConfigDatas
{
    public class Edit
    {
        public class Command : IRequest
        {
            public WebConfigData WebConfigData { get; set; }
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
                return await _context.WebConfigDatas.AnyAsync(u => u.Id == id);
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var webdata = await _context.WebConfigDatas.FirstOrDefaultAsync(u => u.Id == request.WebConfigData.Id);

                if (webdata == null)
                {
                    throw new Exception("ID " + request.WebConfigData.Id + " tidak ditemukan.");
                }

                // Update properti satu per satu
                webdata.WebTitle = request.WebConfigData.WebTitle;
                webdata.WebDescription = request.WebConfigData.WebDescription;
                webdata.EmailRegisterTitle = request.WebConfigData.EmailRegisterTitle;
                webdata.EmailRegisterBody = request.WebConfigData.EmailRegisterBody;
                webdata.EmailInfoTitle = request.WebConfigData.EmailInfoTitle;
                webdata.EmailInfoBody = request.WebConfigData.EmailInfoBody;

                // Simpan perubahan ke dalam konteks
                await _context.SaveChangesAsync();

                return Unit.Value;
            }




        }
    }
}