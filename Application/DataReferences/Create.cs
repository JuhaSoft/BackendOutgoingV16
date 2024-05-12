using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Model;
using MediatR;
using Persistence;

namespace Application.DataReferences
{
    public class Create
    {
        public class Command : IRequest {
            public DataReference DataReference { get; set; }
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
                if (_context.DataReferences.Any(dl => dl.RefereceName == request.DataReference.RefereceName))
                {
                    throw new Exception("Reference  sudah terdaftar.");
                }


                var datareference = new DataReference
                {
                    Id = Guid.NewGuid(),
                    RefereceName = request.DataReference.RefereceName,
                    PsnPos = request.DataReference.PsnPos,
                    RefPos = request.DataReference.RefPos,
                    RefCompare = request.DataReference.RefCompare,
                    StationID = request.DataReference.StationID,
                    isDeleted =false,
                    DataReferenceParameterChecks = new List<DataReferenceParameterCheck>()
                };
                int order = 1;
                foreach (var datareferenceParameter in request.DataReference.DataReferenceParameterChecks)
                {
                   

                    // Buat ParameterCheckErrorMessage baru
                    var datareferenceParamcheck = new DataReferenceParameterCheck
                    {
                        Id = Guid.NewGuid(),
                        DataReferenceId = datareference.Id,
                        ParameterCheckId = datareferenceParameter.ParameterCheckId,
                        Order = order
                    };

                    // Tambahkan ke koleksi ParameterCheckErrorMessages pada ParameterCheck
                    datareference.DataReferenceParameterChecks.Add(datareferenceParamcheck);
                    order++;
                }
                await _context.DataReferences.AddAsync(datareference);
                //_context.DataReferences.Add(request.DataReference);
                await _context.SaveChangesAsync();
                return Unit.Value;
            }

           
        }

    }
}