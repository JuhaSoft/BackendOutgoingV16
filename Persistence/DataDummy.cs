using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class DataDummy
    {
         public static async Task SeedAllData(DataContext context, UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any()){
                var Users= new List<AppUser>{
                new AppUser{DisplayName= "juha",UserName="Juhaway"},
                new AppUser{DisplayName= "juha1",UserName="Juhaway1"},
                new AppUser{DisplayName= "juha2",UserName="Juhaway2"},
                new AppUser{DisplayName= "juha3",UserName="Juhaway3"}
                };
                foreach(var user in Users){
                    await userManager.CreateAsync(user,"Pa$$w0rd");
                }
                
            }

            //var parameterCheck2 = new List<ParameterCheck>
            //{
            //    new ParameterCheck

            //    {
            //         Id = Guid.NewGuid(),
            //         Description="Cek Label",
            //       Order=1,
            //       DataReferenceId=Guid.Parse("348D3DEA-D647-4925-9784-08DC5D5B23D3"),
            //    },
            //     new ParameterCheck
            //    {
            //         Id = Guid.NewGuid(),
            //         Description="Cek LED",
            //         Order=2,
            //       DataReferenceId=Guid.Parse("348D3DEA-D647-4925-9784-08DC5D5B23D3"),
            //    },
            //    new ParameterCheck
            //    {
            //         Id = Guid.NewGuid(),
            //         Description="Cek Cover",
            //         Order=3,
            //       DataReferenceId=Guid.Parse("348D3DEA-D647-4925-9784-08DC5D5B23D3"),
            //    },
            //    new ParameterCheck
            //    {
            //         Id = Guid.NewGuid(),
            //         Description="Cek Label",
            //         Order=1,
            //       DataReferenceId=Guid.Parse("5B93A59C-9987-4FE8-9785-08DC5D5B23D3"),
            //    },



            //};

            //await context.ParameterChecks.AddRangeAsync(parameterCheck2);
            //await context.SaveChangesAsync();

            if (context.DataLines.Any()) return;

            var dataLine = new DataLine
            {
                Id = Guid.NewGuid(),
                LineId = "Line1",
                LineName = "Line One",
                isDeleted = false
            };

            // Menambahkan DataLine ke konteks
            context.DataLines.Add(dataLine);
            context.SaveChanges();

            // Membuat daftar DataReference baru
            var dataReferences = new List<DataReference>
			{
				new DataReference
				{
					Id = Guid.NewGuid(),
					RefereceName = "Reference 1",
					StationID =  Guid.NewGuid(), // ID dari Line
					isDeleted = false
				},
				new DataReference
				{
					Id = Guid.NewGuid(),
					RefereceName = "Reference 2",
                    StationID = dataLine.Id, // ID dari Line yang sama
					isDeleted = false
				},
				// Tambahkan entitas DataReference tambahan di sini jika diperlukan
			};

            // Menambahkan semua entitas DataReference ke konteks dan menyimpan perubahan
            foreach (var dataReference in dataReferences)
            {
                context.DataReferences.Add(dataReference);
            }
            await context.DataReferences.AddRangeAsync(dataReferences);
            context.SaveChanges();
            var referenceIds = new List<Guid>();

            // Menambahkan ID referensi ke dalam array
            foreach (var dataReference in dataReferences)
            {
                referenceIds.Add(dataReference.Id);
            }

            var LineIDL = context.DataLines.FirstOrDefault()?.Id;

            var laststationIDs = new List<LastStationID>();

           foreach (var referenceId in referenceIds)
            {
         var DatalaststationID = new LastStationID
                {
                    Id = Guid.NewGuid(),
                    StationID = "Station1",
                    LineId = (Guid)LineIDL,
                };

                laststationIDs.Add(DatalaststationID);
            }
            await context.LastStationIDs.AddRangeAsync(laststationIDs);
            await context.SaveChangesAsync();
			
			
            var SelectOptions = new List<SelectOption>
            {
                new SelectOption
                {
                    Id = Guid.NewGuid(),
                    PCID = "affsffgsf2f2gfg3fg",
                    SOptionValue ="Combo",

                },
                new SelectOption
                {
                    Id = Guid.NewGuid(),
                    PCID = "jhshhjshdyytydgd",
                    SOptionValue = "Option",

                },
                new SelectOption
                {
                    Id = Guid.NewGuid(),
                    PCID = "hgdhghdgytyeghsghgy",
                    SOptionValue = "TextBox",

                }
            };

            await context.SelectOptions.AddRangeAsync(SelectOptions);

            await context.SaveChangesAsync();
			
			string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            
			var parameterCheck = new List<ParameterCheck>
            {
                new ParameterCheck

                {
                     Id = Guid.NewGuid(),
                   Order=1,
                   DataReferenceId=Guid.Parse("5F467111-EE88-410C-52BA-08DC3840975C"),
                },
                 new ParameterCheck
                {
                     Id = Guid.NewGuid(),
                     Order=2,
                   DataReferenceId=Guid.Parse("5F467111-EE88-410C-52BA-08DC3840975C"),
                },
                new ParameterCheck
                {
                     Id = Guid.NewGuid(),
                     Order=3,
                   DataReferenceId=Guid.Parse("5F467111-EE88-410C-52BA-08DC3840975C"),
                },
                new ParameterCheck
                {
                     Id = Guid.NewGuid(),
                     Order=1,
                   DataReferenceId=Guid.Parse("348D3DEA-D647-4925-9784-08DC5D5B23D3"),
                },



            };

            await context.ParameterChecks.AddRangeAsync(parameterCheck);
            await context.SaveChangesAsync();
			var lastStationId = await context.LastStationIDs.FirstOrDefaultAsync(); // Mengambil entitas LastStationID yang pertama

			var dataTracks = new List<DataTrack>
			{
				new DataTrack
				{
					TrackPSN = "PSN11111",
					TrackReference ="RefA",
					TrackingWO = "WO1",
					TrackingLastStationId = lastStationId.Id, // Menggunakan ID dari entitas LastStationID yang pertama
					TrackingDateCreate = DateTime.UtcNow,
					TrackingResult = "Pass",
					TrackingStatus = "Pass",
					DataTrackCheckings = new List<DataTrackChecking>
					{
						new DataTrackChecking
						{
							PCID = parameterCheck[0].Id, // Menggunakan ID dari parameter check yang pertama
							DTCValue = "Value1",
							ImageDataChecks = new List<ImageDataCheck>
							{
								new ImageDataCheck
								{
									ImageUrl = "ImageUrl1"
								},
								new ImageDataCheck
								{
									ImageUrl = "ImageUrl2"
								}
							}
						},
						new DataTrackChecking
						{
							PCID = parameterCheck[1].Id, // Menggunakan ID dari parameter check yang kedua
							DTCValue = "Value2",
							ImageDataChecks = new List<ImageDataCheck>
							{
								new ImageDataCheck
								{
									ImageUrl = "ImageUrl3"
								},
								new ImageDataCheck
								{
									ImageUrl = "ImageUrl4"
								}
							}
						}
					}
				},
					// Tambahkan data track tambahan di sini jika diperlukan
				};

				// Simpan data dummy DataTrack
				await context.DataTracks.AddRangeAsync(dataTracks);
				await context.SaveChangesAsync();

        }
    }
}