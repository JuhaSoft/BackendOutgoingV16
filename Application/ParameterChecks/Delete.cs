using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace Application.ParameterChecks
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IHostingEnvironment _hostingEnvironment;

            public Handler(DataContext context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment)
            {
                _context = context;
                _httpContextAccessor = httpContextAccessor;
                _hostingEnvironment = hostingEnvironment;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                // Cari ParameterCheck berdasarkan request.Id
                var paramCheck = await _context.ParameterChecks.FindAsync(request.Id);
                if (paramCheck == null)
                {
                    throw new Exception("ParameterCheckId tidak ditemukan.");
                }

                // Ambil data ParameterChecks beserta ParameterCheckErrorMessages
                var dbParamCheck = await _context.ParameterChecks
                    .Include(pc => pc.ParameterCheckErrorMessages)
                    .SingleOrDefaultAsync(pc => pc.Id == request.Id, cancellationToken);

                // Hapus gambar jika ada
                
              
                var uploadsFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "Upload", "Image", "ParameterCheck");
                if (Directory.Exists(uploadsFolderPath))
                {
                    if (File.Exists(dbParamCheck.ImageSampleUrl))
                    {
                        File.Delete(dbParamCheck.ImageSampleUrl);
                    }
                    //File.Delete(dbParamCheck.ImageSampleUrl);
                }
                // Hapus data ParameterCheckErrorMessages yang terkait
                _context.ParameterCheckErrorMessages.RemoveRange(dbParamCheck.ParameterCheckErrorMessages);

                // Hapus data ParameterChecks
                _context.ParameterChecks.Remove(dbParamCheck);

                // Simpan perubahan
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

        }
    }
}
