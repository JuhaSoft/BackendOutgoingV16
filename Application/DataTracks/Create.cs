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

namespace Application.DataTracks
{
    public class Create
    {
        public class Command : IRequest
        {
            public DataTrack Request { get; set; }
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
                var dataTrack = new DataTrack
                {
                    Id = Guid.NewGuid(),
                    TrackPSN = request.Request.TrackPSN,
                    TrackReference = request.Request.TrackReference,
                    TrackingWO = request.Request.TrackingWO,
                    TrackingLastStationId = Guid.Parse(request.Request.TrackingLastStationId.ToString().Replace("{", "").Replace("}", "")),
                    TrackingResult = request.Request.TrackingResult,
                    TrackingStatus = request.Request.TrackingStatus,
                    TrackingUserIdChecked = request.Request.TrackingUserIdChecked,
                    TrackingDateCreate = DateTime.Now,
                    DataTrackCheckings = new List<DataTrackChecking>()
                };

                bool hasError = false;

                foreach (var dtc in request.Request.DataTrackCheckings)
                {
                    var dataTrackChecking = new DataTrackChecking
                    {
                        Id = Guid.NewGuid(),
                        DataTrackID = dataTrack.Id,
                        PCID = Guid.Parse(dtc.PCID.ToString().Replace("{", "").Replace("}", "")),
                        DTCValue = dtc.DTCValue,
                        DTCisDeleted = dtc.DTCisDeleted,
                        ImageDataChecks = new List<ImageDataCheck>()
                    };

                    foreach (var image in dtc.ImageDataChecks)
                    {
                        try
                        {
                            // Decode base64 string menjadi byte array
                            byte[] imageBytes = Convert.FromBase64String(image.ImageUrl.Split(',')[1]);
                            int imageSizeInBytes = imageBytes.Length;

                            // Konversi ke kilobyte
                            double imageSizeInKb = imageSizeInBytes / 1024.0;

                            // Konversi ke megabyte
                            double imageSizeInMb = imageSizeInKb / 1024.0;

                            Console.WriteLine($"Ukuran gambar: {imageSizeInBytes} bytes, {imageSizeInKb} KB, {imageSizeInMb} MB");

                            // Buat nama file unik untuk gambar
                            string fileName = Guid.NewGuid().ToString() + ".jpg";

                            // Jalur lengkap di mana file gambar akan disimpan
                            var uploadsPath = Path.Combine("Upload", "Image", dataTrack.TrackingWO);
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

                            var imageDataCheck = new ImageDataCheck
                            {
                                Id = Guid.NewGuid(),
                                ImageUrl = $"/{uploadsPath}/{fileName}",
                                DataTrackCheckingId = dataTrackChecking.Id
                            };

                            dataTrackChecking.ImageDataChecks.Add(imageDataCheck);
                        }
                        catch (Exception ex)
                        {
                            // Tangani exception di sini
                            Console.WriteLine($"Error saat menyimpan gambar: {ex.Message}");
                            hasError = true;
                        }
                    }

                    if (!hasError)
                    {
                        dataTrack.DataTrackCheckings.Add(dataTrackChecking);
                    }
                }

                if (!hasError)
                {
                    _context.DataTracks.Add(dataTrack);
                    await _context.SaveChangesAsync();
                }

                return Unit.Value;
            }
        }
    }
}
