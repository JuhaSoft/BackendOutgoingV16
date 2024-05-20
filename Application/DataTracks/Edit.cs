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
                var TrackingResultOld = "";
                var datatrack = await _context.DataTracks
                    .Include(dt => dt.DataTrackCheckings)
                        .ThenInclude(dtc => dtc.ImageDataChecks)
                    .FirstOrDefaultAsync(u => u.Id == request.DataTrack.Id);

                if (datatrack == null)
                {
                    throw new Exception("Data track tidak ditemukan.");
                }
                else
                {
                    TrackingResultOld = datatrack.TrackingResult;
                }
                bool hasError = false;

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
                await _context.SaveChangesAsync(cancellationToken); // Ensure the changes are saved

                // Validasi ErrorId sebelum melakukan insert dan ganti Guid.Empty dengan null
                var errorIds = request.DataTrack.DataTrackCheckings.Select(dtc => dtc.ErrorId).Distinct().ToList();
                var validErrorIds = await _context.ErrorMessages
                    .Where(em => errorIds.Contains(em.Id))
                    .Select(em => em.Id)
                    .ToListAsync(cancellationToken);
                Guid? approvalId = null;
                bool? approver = false;
                foreach (var requestDtc in request.DataTrack.DataTrackCheckings)
                {
                    // Ubah Guid.Empty menjadi null
                    if (requestDtc.ErrorId == Guid.Empty)
                    {
                        requestDtc.ErrorId = null;
                    }

                    if (requestDtc.ErrorId != null && !validErrorIds.Contains(requestDtc.ErrorId.Value))
                    {
                        throw new Exception($"ErrorId {requestDtc.ErrorId} tidak ditemukan di tabel ErrorMessages.");
                    }
                    if (requestDtc.Approve)
                    {
                        // Konversi request.DataTrack.TrackingUserIdChecked menjadi Guid
                        Guid userIdChecked;
                        if (Guid.TryParse(request.DataTrack.TrackingUserIdChecked, out userIdChecked))
                        {
                            approvalId = userIdChecked;
                            approver = true;
                        }
                        else
                        {
                            // Jika konversi gagal, Anda dapat menentukan tindakan yang sesuai, seperti melempar exception atau menangani kasus tersebut.
                            throw new Exception("Nilai request.DataTrack.TrackingUserIdChecked tidak valid.");
                        }
                    }
                    var dtc = new DataTrackChecking
                    {
                        DataTrackID = datatrack.Id,
                        PCID = requestDtc.PCID,
                        DTCValue = requestDtc.DTCValue,
                        ErrorId = requestDtc.ErrorId,
                        ApprovalId = approvalId?.ToString(),
                        ApprRemaks = requestDtc.ApprRemaks,
                        Approve = requestDtc.Approve,
                        ImageDataChecks = new List<ImageDataCheck>()
                    };
                    datatrack.ApprovalId = approvalId?.ToString();
                    //datatrack.Approve = approver;
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

                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    hasError = true;
                    // Optionally, you can log the error or handle it in another way
                }

                if (!hasError)
                {
                    // Update DataTrack
                    _mapper.Map(request.DataTrack, datatrack);
                    //datatrack.TrackingDateCreate = DateTime.Now;
                    datatrack.TrackingResult = request.DataTrack.TrackingResult;
                    datatrack.TrackingStatus = request.DataTrack.TrackingStatus;

                    await _context.SaveChangesAsync(cancellationToken);
                }

                var workOrder = await _context.WorkOrders.FirstOrDefaultAsync(wo => wo.WoNumber == datatrack.TrackingWO, cancellationToken);

                if (workOrder != null)
                {
                    int failQty;
                    int passQty;

                    // Periksa apakah workOrder.FailQTY bisa diubah menjadi bilangan bulat
                    if (int.TryParse(workOrder.FailQTY, out failQty))
                    {
                        workOrder.FailQTY = (failQty).ToString();
                    }
                    else
                    {
                        workOrder.FailQTY = "0";
                    }

                    // Periksa apakah workOrder.PassQTY bisa diubah menjadi bilangan bulat
                    if (int.TryParse(workOrder.PassQTY, out passQty))
                    {
                        workOrder.PassQTY = (passQty).ToString();
                    }
                    else
                    {
                        workOrder.PassQTY = "0";
                    }
                    // Cek apakah TrackingResult diubah dari Fail ke Pass
                    if (TrackingResultOld == "FAIL" && datatrack.TrackingResult == "PASS")
                    {
                        if (int.Parse(workOrder.FailQTY) < 1)
                        {
                            workOrder.FailQTY = "0";
                        }
                        else
                        {
                            workOrder.FailQTY = (int.Parse(workOrder.FailQTY) - 1).ToString();
                        }

                        workOrder.PassQTY = (int.Parse(workOrder.PassQTY) + 1).ToString();
                    }
                    // Cek apakah TrackingResult diubah dari Pass ke Fail
                    else if (TrackingResultOld == "PASS" && datatrack.TrackingResult == "FAIL")
                    {
                        workOrder.FailQTY = (int.Parse(workOrder.FailQTY) + 1).ToString();
                        if (int.Parse(workOrder.PassQTY) < 1)
                        {
                            workOrder.PassQTY = "0";
                        }
                        else
                        {
                            workOrder.PassQTY = (int.Parse(workOrder.PassQTY) - 1).ToString();
                        }
                    }

                    // Simpan perubahan pada WorkOrder
                    await _context.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }




        }
    }
}