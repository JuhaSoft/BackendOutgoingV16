using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using BCrypt.Net;
namespace Persistence
{
    public class Seed
    {
        public static async Task SeedLineData(DataContext context)
        {
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
            StationID = dataLine.Id, // ID dari Line
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



        }


        public static async Task SeedPCData(DataContext context)
        {
            if (context.SelectOptions.Any()) return;

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
        }
        
        public static async Task SeedDatatrackData(DataContext context)
        {

            // Hash password menggunakan salt yang baru dibuat

            if (context.DataTracks.Any()) return;

            var dataTracks = new List<DataTrack>
            {
                new DataTrack

                {
                    TrackPSN = "PSN11111",
                    TrackReference ="RefA",
                     TrackingWO = "WO1",
                     TrackingLastStationId=Guid.NewGuid(),
                     TrackingDateCreate=DateTime.UtcNow,
                     TrackingResult="Pass",
                     TrackingStatus="Pass"
                },
                new DataTrack

                {
                    TrackPSN = "PSN11112",
                    TrackReference ="RefA",
                     TrackingWO = "WO1",
                     TrackingLastStationId=Guid.NewGuid(),
                     TrackingDateCreate=DateTime.UtcNow,
                     TrackingResult="Pass",
                     TrackingStatus="Pass"
                },
                new DataTrack

                {
                    TrackPSN = "PSN11113",
                    TrackReference ="RefA",
                     TrackingWO = "WO1",
                     TrackingLastStationId=Guid.NewGuid(),
                     TrackingDateCreate=DateTime.UtcNow,
                     TrackingResult="Pass",
                     TrackingStatus="Pass"
                },
                new DataTrack

                {
                    TrackPSN = "PSN11114",
                    TrackReference ="RefA",
                     TrackingWO = "WO1",
                     TrackingLastStationId=Guid.NewGuid(),
                     TrackingDateCreate=DateTime.UtcNow,
                     TrackingResult="Pass",
                     TrackingStatus="Pass"
                },
                new DataTrack

                {
                    TrackPSN = "PSN11115",
                    TrackReference ="RefA",
                     TrackingWO = "WO1",
                     TrackingLastStationId=Guid.NewGuid(),
                     TrackingDateCreate=DateTime.UtcNow,
                     TrackingResult="Pass",
                     TrackingStatus="Pass"
                },

               new DataTrack

                {
                    TrackPSN = "PSN11121",
                    TrackReference ="RefB",
                     TrackingWO = "WO3",
                     TrackingLastStationId=Guid.NewGuid(),
                     TrackingDateCreate=DateTime.UtcNow,
                     TrackingResult="Pass",
                     TrackingStatus="Pass"
                },
                new DataTrack

                {
                    TrackPSN = "PSN11122",
                    TrackReference ="RefB",
                     TrackingWO = "WO3",
                     TrackingLastStationId=Guid.NewGuid(),
                     TrackingDateCreate=DateTime.UtcNow,
                     TrackingResult="Pass",
                     TrackingStatus="Pass"
                },
                new DataTrack

                {
                    TrackPSN = "PSN11123",
                    TrackReference ="RefB",
                     TrackingWO = "WO3",
                     TrackingLastStationId=Guid.NewGuid(),
                     TrackingDateCreate=DateTime.UtcNow,
                     TrackingResult="Pass",
                     TrackingStatus="Pass"
                },
                new DataTrack

                {
                    TrackPSN = "PSN11124",
                    TrackReference ="RefB",
                     TrackingWO = "WO3",
                     TrackingLastStationId=Guid.NewGuid(),
                     TrackingDateCreate=DateTime.UtcNow,
                     TrackingResult="Pass",
                     TrackingStatus="Pass"
                },
            };

            await context.DataTracks.AddRangeAsync(dataTracks);
            await context.SaveChangesAsync();
        }
        public static async Task SeedDataType(DataContext context)
        {
            if (context.DataContrplTypes.Any()) return;

            var dataContrplTypesD = new List<DataContrplType>
            {
                new DataContrplType
                {
                 Id = Guid.NewGuid(),
                    CTName =DataControl.Combo.ToString(),

                },
                new DataContrplType
                {
                  Id = Guid.NewGuid(),
                    CTName =DataControl.Select.ToString(),

                },
                new DataContrplType
                {
                  Id = Guid.NewGuid(),
                    CTName =DataControl.SelectOption.ToString(),

                },
                 new DataContrplType
                {
                  Id = Guid.NewGuid(),
                    CTName =DataControl.Data.ToString(),

                },
            };

            await context.DataContrplTypes.AddRangeAsync(dataContrplTypesD);

            await context.SaveChangesAsync();
        }

         


        
        public static async Task SeedDTCheckImageData(DataContext context)
        {

            // Hash password menggunakan salt yang baru dibuat

            if (context.ImageDataChecks.Any()) return;

            var dataImage = new List<ImageDataCheck>
            {
                new ImageDataCheck
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = "TestUrl1",
                    DataTrackCheckingId=Guid.Parse("9155AB10-33A2-4B17-BC44-021245C46DAB"),
                },
                 new ImageDataCheck
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = "TestUrl2",
                    DataTrackCheckingId=Guid.Parse("9155AB10-33A2-4B17-BC44-021245C46DAB"),
                },

            };

            await context.ImageDataChecks.AddRangeAsync(dataImage);
            await context.SaveChangesAsync();
        }

        public static async Task SeedDataTrackFullData(DataContext context)
        {

            // Hash password menggunakan salt yang baru dibuat

            if (context.DataTracks.Any()) return;

            // Persiapan data dummy untuk DataTrack
            var dataTracks = new List<DataTrack>
            {
                new DataTrack
                {
                    TrackPSN = "PSN11111",
                    TrackReference ="RefA",
                    TrackingWO = "WO1",
                    TrackingLastStationId = Guid.NewGuid(),
                    TrackingDateCreate = DateTime.UtcNow,
                    TrackingResult = "Pass",
                    TrackingStatus = "Pass",
                    DataTrackCheckings = new List<DataTrackChecking>
                    {
                        new DataTrackChecking
                        {
                            PCID = Guid.NewGuid(), // Sesuaikan dengan parameter check yang sudah dibuat
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
                            PCID = Guid.NewGuid(), // Sesuaikan dengan parameter check yang sudah dibuat
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
                // DataTrack lainnya...
            };

            // Simpan data dummy DataTrack
            await context.DataTracks.AddRangeAsync(dataTracks);
            await context.SaveChangesAsync();

        }











    }
}