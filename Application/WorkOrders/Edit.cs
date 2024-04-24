using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Model;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.WorkOrders
{
    public class Edit
    {
         public class Command : IRequest 
        {
            public WorkOrder   WorkOrder { get; set; }
        }
    public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x=>x.WorkOrder).SetValidator(new WOValidator());

            }
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
                return await _context.WorkOrders.AnyAsync(u => u.Id == id);
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
               
                if (!await IsCTUnique(request.WorkOrder.Id))

                {
                    // Username sudah ada, Anda dapat menangani kasus ini sesuai kebutuhan aplikasi Anda.
                    // Misalnya, lempar pengecualian, kirimkan respons yang sesuai, dll.
                    throw new Exception("ID Untuk Work Order" + request.WorkOrder.Id + "' Tidak ditemukan.");
                }
        
                var workorder = await _context.WorkOrders.FirstOrDefaultAsync(u => u.Id == request.WorkOrder.Id);
                //_mapper.Map(request.WorkOrder,workorder);
                
                workorder.WoNumber = request.WorkOrder.WoNumber;
                workorder.SONumber = request.WorkOrder.SONumber;
                workorder.WoReferenceID = request.WorkOrder.WoReferenceID;

                workorder.WoQTY = request.WorkOrder.WoQTY;
                workorder.PassQTY = request.WorkOrder.PassQTY;
                workorder.FailQTY = request.WorkOrder.FailQTY;
                workorder.WoStatus = request.WorkOrder.WoStatus;

                await _context.SaveChangesAsync();
            return Unit.Value;
            }

           
        }
    }
}