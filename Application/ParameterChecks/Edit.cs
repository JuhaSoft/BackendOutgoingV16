using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common.DTOs;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace Application.ParameterChecks
{
    public class Edit
    {
        public class Command : IRequest<Unit>
        {
            public Guid ParamID { get; set; }
            public ParameterCheck ParameterChecks { get; set; }
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
                var paramCheck = await _context.ParameterChecks.FindAsync(request.ParamID);
                if (paramCheck == null)
                {
                    throw new Exception("ParameterCheckId tidak ditemukan.");
                }

                var dbParamCheck = await _context.ParameterChecks
                    .Include(pc => pc.ParameterCheckErrorMessages)
                    .ThenInclude(pcem => pcem.ErrorMessage)
                    .SingleOrDefaultAsync(pc => pc.Id == request.ParamID, cancellationToken);

                if (Uri.TryCreate(request.ParameterChecks.ImageSampleUrl, UriKind.Absolute, out Uri uriResult))
                {
                    request.ParameterChecks.ImageSampleUrl = request.ParameterChecks.ImageSampleUrl.Replace($"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}/", "/");
                }
                else
                {
                    var imagedelete = Path.Combine(_hostingEnvironment.WebRootPath, dbParamCheck.ImageSampleUrl.TrimStart('/'));
                    Console.WriteLine($"ImageSampleUrl berupa Gambar: {request.ParameterChecks.ImageSampleUrl}");

                    var uploadsFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "Upload", "Image", "ParameterCheck");
                    if (Directory.Exists(uploadsFolderPath))
                    {
                        File.Delete(imagedelete);
                    }

                    var uploadsPath = Path.Combine("Upload", "Image", "ParameterCheck");
                    var webRootPath = _hostingEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var uploadsFolder = Path.Combine(webRootPath, uploadsPath);
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + ".jpg";
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    var bytes = Convert.FromBase64String(request.ParameterChecks.ImageSampleUrl.Split(',')[1]);
                    await File.WriteAllBytesAsync(filePath, bytes);

                    uploadsPath = "/" + uploadsPath;
                    request.ParameterChecks.ImageSampleUrl = Path.Combine(uploadsPath, fileName);
                }

                // 1. Update domain ParameterCheck
                _context.Entry(dbParamCheck).CurrentValues.SetValues(request.ParameterChecks);

                // 2. Hapus data lama di ParameterCheckErrorMessages
                _context.ParameterCheckErrorMessages.RemoveRange(
                    _context.ParameterCheckErrorMessages.Where(pcem => pcem.ParameterCheckId == request.ParamID)
                );

                // 3. Masukkan data baru ke ParameterCheckErrorMessages
              
                foreach (var errorCodeDto in request.ParameterChecks.ParameterCheckErrorMessages)
                {
                    dbParamCheck.ParameterCheckErrorMessages.Add(new ParameterCheckErrorMessage
                    {
                        ParameterCheckId = request.ParamID,
                        ErrorMessageId = errorCodeDto.ErrorMessageId,
                        Order = errorCodeDto.Order
                    });
                }

                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}