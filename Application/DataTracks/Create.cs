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
using System.Net.Mail;
using System.Net;
using Common.Hubs;
using Microsoft.AspNetCore.SignalR;

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
            private readonly IHubContext<NotificationHub> _hubContext;
            public Handler(DataContext context, IWebHostEnvironment hostingEnvironment, IHubContext<NotificationHub> hubContext)
            {
                _context = context;
                _hostingEnvironment = hostingEnvironment;
                _hubContext = hubContext;
            }
            private async Task<List<string>> GetEmailsAsync()
            {
                var staffEmails = await _context.Users
                    .Where(u => u.Role == "Admin")
                    //.Where(u => u.Role == "staff")
                    .Select(u => u.Email)
                    .ToListAsync();

                return staffEmails;
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
                    if (dtc.DTCValue != "Pass" && dtc.Approve == true)
                    {
                        dataTrack.ApprovalId = request.Request.TrackingUserIdChecked;


                    }
                    var dataTrackChecking = new DataTrackChecking
                    {
                        Id = Guid.NewGuid(),
                        DataTrackID = dataTrack.Id,
                        PCID = Guid.Parse(dtc.PCID.ToString().Replace("{", "").Replace("}", "")),
                        DTCValue = dtc.DTCValue,
                        Approve=dtc.Approve,
                        ApprovalId= dtc.Approve != true ? null: dataTrack.TrackingUserIdChecked,
                        ErrorId = dtc.DTCValue != "Fail" ? null : dtc.ErrorId,
                        DTCisDeleted = dtc.DTCisDeleted,
                        ApprRemaks= dtc.ApprRemaks,
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
                    try
                    {
                        _context.DataTracks.Add(dataTrack);
                        await _context.SaveChangesAsync();
                        Console.WriteLine("Sending DataUpdated broadcast...");
                        
                        //email
                        List<string> staffEmails = await GetEmailsAsync();

                        // Konfigurasikan pengaturan SMTP
                        var smtpClient = new SmtpClient("smtp.gmail.com")
                        {
                            Port = 587,
                            Credentials = new NetworkCredential("gugai.way@gmail.com", "pktb dsso aeeb wewf"),
                            EnableSsl = true
                        };




                        foreach (var dtce in request.Request.DataTrackCheckings)
                        {
                            if(dtce.DTCValue != "Pass")
                            {

                                foreach (var email in staffEmails)
                                {
                                    var mailMessage = new MailMessage
                                    {
                                        From = new MailAddress("gugai.way@gmail.com"),
                                        Subject = "Kesalahan Ditemukan dalam Pemeriksaan Data",
                                        Body = $"Kesalahan ditemukan dalam pemeriksaan data untuk PSN: {dataTrack.TrackPSN}. Silakan periksa detail di sistem."
                                    };
                                    mailMessage.To.Add(new MailAddress(email));

                                    try
                                    {
                                        await smtpClient.SendMailAsync(mailMessage);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Gagal mengirim email ke {email}: {ex.Message}");
                                    }
                                }
                                ErrorTrack errorTrack = new ErrorTrack
                                {
                                    Id = Guid.NewGuid(),
                                    TrackPSN = dataTrack.TrackPSN,
                                    TrackingDateCreate = dataTrack.TrackingDateCreate,
                                    PCID = Guid.Parse(dtce.PCID.ToString().Replace("{", "").Replace("}", "")),
                                    //ParameterCheck = null, // Ganti dengan ParameterCheck yang sesuai
                                    ErrorId = Guid.Parse(dtce.ErrorId.ToString().Replace("{", "").Replace("}", "")),
                                    //ErrorMessage = null // Ganti dengan ErrorMessage yang sesuai
                                };

                                // Simpan objek ErrorTrack ke dalam basis data
                                _context.ErrorTrack.Add(errorTrack);
                                await _context.SaveChangesAsync();
                            }
                        }
                            // Cek apakah TrackingStatus tidak sama dengan "Pass"
                            
                    }
                    catch (Exception ex)
                    {
                        // Tangani exception di sini
                        Console.WriteLine($"Error saat menyimpan data: {ex.Message}");
                        Console.WriteLine(ex.InnerException.Message);
                    }
                }
                await _hubContext.Clients.All.SendAsync("ReceiveUpdateDataNotification", "Data has been updated.");
                var workOrder = await _context.WorkOrders.FirstOrDefaultAsync(wo => wo.WoNumber == dataTrack.TrackingWO, cancellationToken);

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
                    if ( dataTrack.TrackingResult == "PASS")
                    {
                        if (int.Parse(workOrder.FailQTY) < 1)
                        {
                            workOrder.FailQTY = "0";
                        }
                        else
                        {
                            workOrder.FailQTY = (int.Parse(workOrder.FailQTY)).ToString();
                        }

                        workOrder.PassQTY = (int.Parse(workOrder.PassQTY) + 1).ToString();
                    }
                    // Cek apakah TrackingResult diubah dari Pass ke Fail
                    else if ( dataTrack.TrackingResult == "FAIL")
                    {
                        workOrder.FailQTY = (int.Parse(workOrder.FailQTY) + 1).ToString();
                        if (int.Parse(workOrder.PassQTY) < 1)
                        {
                            workOrder.PassQTY = "0";
                        }
                        else
                        {
                            workOrder.PassQTY = (int.Parse(workOrder.PassQTY)).ToString();
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
