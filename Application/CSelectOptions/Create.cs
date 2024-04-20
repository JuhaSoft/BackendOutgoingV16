using System.Threading;
using System.Threading.Tasks;
using Domain.Model;
using MediatR;
using Persistence;

namespace Application.CSelectOptions
{
    public class Create
    {
             public class Command : IRequest {
            public SelectOption SelectOption { get; set; }
        }
  
        public class Handler : IRequestHandler<Command>
        {
 
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
            this._context = context;
            }

         
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
             
                _context.SelectOptions.Add(request.SelectOption);
                await _context.SaveChangesAsync();
                return Unit.Value;
            }

           
        }
    }
}