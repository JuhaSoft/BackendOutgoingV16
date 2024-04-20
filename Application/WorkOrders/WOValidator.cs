using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using FluentValidation;

namespace Application.WorkOrders
{
    public class WOValidator : AbstractValidator<WorkOrder>
    {
        public WOValidator()
        {
            RuleFor(x => x.WoStatus).NotEmpty();
            RuleFor(x => x.WoNumber).NotEmpty();
            RuleFor(x => x.WoNumber).MinimumLength(4);
            RuleFor(x => x.WoNumber).MaximumLength(12);
            RuleFor(x => x.SONumber).NotEmpty();
            RuleFor(x => x.SONumber).MinimumLength(4);
            RuleFor(x => x.SONumber).MaximumLength(12);
            RuleFor(x => x.WoReferenceID).NotEmpty();
            RuleFor(x => x.WoQTY).NotEmpty().WithMessage("Quantity is required");
            RuleFor(x => x.WoQTY).MinimumLength(0).WithMessage("Quantity must be greater than or equal to 0");
            RuleFor(x => x.WoQTY).MaximumLength(5).WithMessage("Quantity must be less than or equal to 10000");
        }
    }
}