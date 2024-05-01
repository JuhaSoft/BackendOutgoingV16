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
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using Microsoft.AspNetCore.Http;
namespace Application.DataTracks
{
    public class Edit
    {
        public class Command : IRequest 
        {
            public DataTrack DataTrack { get; set; }
        }
  
        public class Handler : IRequestHandler<Command>
        {
        private readonly IMapper _mapper;
 
        private readonly DataContext _context;
            private readonly IWebHostEnvironment _hostingEnvironment;
            private readonly IHttpContextAccessor _httpContextAccessor;
            public Handler(DataContext context, IMapper mapper, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
            {
            this._mapper = mapper;
            this._context = context;
             _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            }

            public async Task<bool> IsPSNUnique(Guid id)
            {
                return await _context.DataTracks.AnyAsync(u => u.Id == id);
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (!await IsPSNUnique(request.DataTrack.Id))
                {
                    throw new Exception("ID Untuk PSN" + request.DataTrack.TrackPSN + "' Tidak ditemukan.");
                }

                var datatrack = await _context.DataTracks
                    .Include(dt => dt.DataTrackCheckings)
                        .ThenInclude(dtc => dtc.ImageDataChecks)
                    .FirstOrDefaultAsync(u => u.Id == request.DataTrack.Id);

                if (datatrack == null)
                {
                    throw new Exception("Data track tidak ditemukan.");
                }

                // Update DataTrack
                _mapper.Map(request.DataTrack, datatrack);

                // Hapus gambar yang tidak digunakan
                //var existingImageChecks = datatrack.DataTrackCheckings.SelectMany(dtc => dtc.ImageDataChecks).ToList();
                //var requestImageUrls = request.DataTrack.DataTrackCheckings.SelectMany(dtc => dtc.ImageDataChecks.Select(img => img.ImageUrl)).ToList();

                //var filesToDelete = existingImageChecks
                //    .Where(img => !string.IsNullOrWhiteSpace(img.ImageUrl)
                //                && Path.IsPathRooted(img.ImageUrl) // Hanya hapus jika ImageUrl adalah path file
                //                && !requestImageUrls.Contains(img.ImageUrl))
                //    .Select(img => Path.Combine(_hostingEnvironment.WebRootPath, img.ImageUrl.TrimStart('/')))
                //    .ToList();

                //foreach (var file in filesToDelete)
                //{
                //    if (File.Exists(file))
                //    {
                //        File.Delete(file);
                //    }
                //}
                // Ambil semua ImageDataCheck dari database yang terkait dengan DataTrackID
                //var existingImageChecks = await _context.DataTrackCheckings
                //    .Where(dtc => dtc.DataTrackID == request.DataTrack.Id)
                //    .SelectMany(dtc => dtc.ImageDataChecks)
                //    .ToListAsync(cancellationToken);

                // Ambil semua ImageUrl dari request
                //var requestImageUrls = request.DataTrack.DataTrackCheckings
                //    .SelectMany(dtc => dtc.ImageDataChecks)
                //    .Select(img => img.ImageUrl)
                //    .ToList();

                //// Bandingkan ImageUrl dan hapus file jika tidak ada di request, kecuali jika bukan path file
                //var filesToDelete = existingImageChecks
                //    .Where(img => !string.IsNullOrWhiteSpace(img.ImageUrl)
                //                && Path.IsPathRooted(img.ImageUrl) // Hanya hapus jika ImageUrl adalah path file
                //                && !requestImageUrls.Contains(img.ImageUrl))
                //    .Select(img => Path.Combine(_hostingEnvironment.WebRootPath, img.ImageUrl.TrimStart('/')))
                //    .ToList();

                //foreach (var file in filesToDelete)
                //{
                //    if (File.Exists(file))
                //    {
                //        File.Delete(file);
                //    }
                //}

                // Ambil semua ImageDataCheck dari database yang terkait dengan DataTrackID
                var existingImageChecks = await _context.DataTrackCheckings
    .Where(dtc => dtc.DataTrackID == request.DataTrack.Id)
    .SelectMany(dtc => dtc.ImageDataChecks)
    .Select(img => img.ImageUrl)
    .ToListAsync(cancellationToken);

                // Ambil semua ImageUrl dari request dan hapus Host dan Port jika ada
                var requestImageUrls = request.DataTrack.DataTrackCheckings
                    .SelectMany(dtc => dtc.ImageDataChecks)
                    .Select(img => img.ImageUrl.Replace($"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}/", "/"))
                    .ToList();

                // Dapatkan daftar file yang harus dihapus (ada di database tapi tidak ada di request)
                var filesToDelete = existingImageChecks
                    .Where(img => !string.IsNullOrWhiteSpace(img)
                                && Path.IsPathRooted(img)
                                && !requestImageUrls.Contains(img))
                    .Select(img => Path.Combine(_hostingEnvironment.WebRootPath, img.TrimStart('/')))
                    .ToList();

                // Dapatkan daftar direktori yang berisi file gambar
                var uploadsFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "Upload", "Image", datatrack.TrackingWO);
                if (Directory.Exists(uploadsFolderPath))
                {
                    foreach (var file in filesToDelete)
                    {
                        File.Delete(file);
                    }
                }


                // Hapus DataTrackCheckings dan ImageDataChecks lama
                var existingDataTrackCheckings = datatrack.DataTrackCheckings.ToList();
                _context.DataTrackCheckings.RemoveRange(existingDataTrackCheckings);

                // Insert DataTrackCheckings dan ImageDataChecks baru
                foreach (var requestDtc in request.DataTrack.DataTrackCheckings)
                {
                    var dtc = new DataTrackChecking
                    {
                        DataTrackID = datatrack.Id,
                        PCID = requestDtc.PCID,
                        DTCValue = requestDtc.DTCValue,
                        ImageDataChecks = new List<ImageDataCheck>() // Inisialisasi ImageDataChecks
                    };

                    foreach (var requestImage in requestDtc.ImageDataChecks)
                    {
                        if (requestImage.ImageUrl.StartsWith("data:image"))
                        {
                            // Simpan gambar baru dan tambahkan ImageDataCheck
                            var uploadsPath = Path.Combine("Upload", "Image", datatrack.TrackingWO);
                            var webRootPath = _hostingEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                            var uploadsFolder = Path.Combine(webRootPath, uploadsPath);
                            Directory.CreateDirectory(uploadsFolder);

                            var fileName = Guid.NewGuid().ToString() + ".jpg";
                            var filePath = Path.Combine(uploadsFolder, fileName);
                            var bytes = Convert.FromBase64String(requestImage.ImageUrl.Split(',')[1]);
                            await File.WriteAllBytesAsync(filePath, bytes);
                            uploadsPath = "/" + uploadsPath;
                            dtc.ImageDataChecks.Add(new ImageDataCheck
                            {
                                
                                ImageUrl = Path.Combine(uploadsPath, fileName)
                            });
                        }
                        else
                        {
                            // Tambahkan ImageDataCheck dengan ImageUrl tanpa alamat server
                            dtc.ImageDataChecks.Add(new ImageDataCheck
                            {
                                ImageUrl = requestImage.ImageUrl.Replace($"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}/", "/")
                            
                            });
                        }
                    }

                    datatrack.DataTrackCheckings.Add(dtc);
                }

                await _context.SaveChangesAsync();

                return Unit.Value;
            }

        }
    }
}