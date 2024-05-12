using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Model;
using MediatR;
using Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Application.ParameterChecks
{
    public class Create
    {
        public class Command : IRequest<Unit>
        {
            public string Description { get; set; }
            public string ImageSampleUrl { get; set; }
            public List<ParameterCheckErrorMessageDto> ParameterCheckErrorMessages { get; set; }
        }
        public class ParameterCheckErrorMessageDto
        {
            public Guid Value { get; set; }
            public string Label { get; set; }
            public int Order { get; set; }
        }
        public class ErrorCodeDto
        {
            public Guid Value { get; set; }
            public string Label { get; set; }
            public int Order { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IWebHostEnvironment _hostingEnvironment;

            public Handler(DataContext context, IWebHostEnvironment hostingEnvironment)
            {
                _context = context;
                _hostingEnvironment = hostingEnvironment;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                // Cek apakah ParameterCheck dengan Description yang sama sudah ada
                if (await _context.ParameterChecks.AnyAsync(pc => pc.Description == request.Description))
                {
                    throw new Exception("Parameter sudah ada.");
                }

                // Buat ParameterCheck baru
                var parameterCheck = new ParameterCheck
                {
                    Id = Guid.NewGuid(),
                    Description = request.Description,
                    ImageSampleUrl = request.ImageSampleUrl,
                    ParameterCheckErrorMessages = new List<ParameterCheckErrorMessage>() // Initialize collection
                };

                // Looping untuk setiap ParameterCheckErrorMessage
                foreach (var parameterCheckErrorDto in request.ParameterCheckErrorMessages)
                {
                    var errorMessageId = parameterCheckErrorDto.Value;

                    // Buat ParameterCheckErrorMessage baru
                    var parameterCheckErrorMessage = new ParameterCheckErrorMessage
                    {
                        Id = Guid.NewGuid(),
                        ParameterCheckId = parameterCheck.Id,
                        ErrorMessageId = errorMessageId,
                        Order = parameterCheckErrorDto.Order
                    };

                    // Tambahkan ke koleksi ParameterCheckErrorMessages pada ParameterCheck
                    parameterCheck.ParameterCheckErrorMessages.Add(parameterCheckErrorMessage);
                }
                try
                {
                    // Decode base64 string menjadi byte array
                    byte[] imageBytes = Convert.FromBase64String(parameterCheck.ImageSampleUrl.Split(',')[1]);
                    int imageSizeInBytes = imageBytes.Length;

                    // Konversi ke kilobyte
                    double imageSizeInKb = imageSizeInBytes / 1024.0;

                    // Konversi ke megabyte
                    double imageSizeInMb = imageSizeInKb / 1024.0;

                    Console.WriteLine($"Ukuran gambar: {imageSizeInBytes} bytes, {imageSizeInKb} KB, {imageSizeInMb} MB");

                    // Buat nama file unik untuk gambar
                    string fileName = Guid.NewGuid().ToString() + ".jpg";

                    // Jalur lengkap di mana file gambar akan disimpan
                    var uploadsPath = Path.Combine("Upload", "Image", "ParameterCheck");
                    var webRootPath = _hostingEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var uploadsFolder = Path.Combine(webRootPath, uploadsPath);

                    // Jika folder belum ada, buat folder tersebut
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Simpan file gambar ke folder
                    var imagePath = Path.Combine(uploadsFolder, fileName);
                    await File.WriteAllBytesAsync(imagePath, imageBytes);


                    parameterCheck.ImageSampleUrl = $"/{uploadsPath}/{fileName}";
                }
                catch (Exception ex)
                {
                    // Tangani exception di sini
                    Console.WriteLine($"Error saat menyimpan gambar: {ex.Message}");
                     
                }
                // Tambahkan ParameterCheck ke context
                await _context.ParameterChecks.AddAsync(parameterCheck);

                // Simpan perubahan ke database
                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
