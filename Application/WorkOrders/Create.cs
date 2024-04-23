using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Domain.Model;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.WorkOrders
{
    public class Create
    {
        public class Command : IRequest<CreateResult> {
            public WorkOrder  WorkOrders { get; set; }
        }

        public class CreateResult
        {
            public bool Success { get; set; }
            public Guid? CreatedWorkOrderId { get; set; }
            public string ErrorMessage { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.WorkOrders).SetValidator(new WOValidator());
            }
        }

        public class Handler : IRequestHandler<Command, CreateResult>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                this._context = context;
            }

            public async Task<CreateResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = new CreateResult();

                try
                {
                    request.WorkOrders.Id = Guid.NewGuid();
                    //var UserID = request.WorkOrders.UserIdCreate.ToString();
                    //request.WorkOrders.UserIdCreate = "47f8f68b-2099-432e-bc87-86e03cb26c9f";
                    _context.WorkOrders.Add(request.WorkOrders);
                    await _context.SaveChangesAsync();

                    result.Success = true;
                    result.CreatedWorkOrderId = request.WorkOrders.Id;
                }
                catch (Exception ex)
                {
                    // Tangani kesalahan jika ada
                    result.Success = false;
                    result.ErrorMessage = ex.Message;
                }

                return result;
            }
        }
    }
}
