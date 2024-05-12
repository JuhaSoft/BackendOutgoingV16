using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ErrorMessage
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Domain.Model.ErrorMessage Errormessage { get; set; }
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
                return await _context.ErrorMessages.AnyAsync(u => u.Id == id);
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var errormessage = await _context.ErrorMessages.FirstOrDefaultAsync(u => u.Id == request.Errormessage.Id);

                if (errormessage == null)
                {
                    throw new Exception("ID " + request.Errormessage.Id + " tidak ditemukan.");
                }

                // Update properti satu per satu
                errormessage.ErrorCode = request.Errormessage.ErrorCode;
                errormessage.ErrorDescription = request.Errormessage.ErrorDescription;

                // Simpan perubahan ke dalam konteks
                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}