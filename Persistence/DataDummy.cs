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
            if (!userManager.Users.Any())
            {
                var Users = new List<AppUser>{
                new AppUser{DisplayName= "juha",UserName="Juhaway",Role="Admin",Email="gugai.way@gmail.com"},

                };
                foreach (var user in Users)
                {
                    await userManager.CreateAsync(user, "Pass1234");
                }

            }
            if (context.WebConfigDatas.Any()) return;

            var webData = new WebConfigData
            {
                Id = Guid.NewGuid(),
                WebTitle = "TQW",
                WebDescription = "TQW",
                EmailRegisterTitle="Application Registed",
                EmailRegisterBody=" Hi, {0} Kamu sudah terdaftar di aplikasi sebagai {2}. Password mu {1}, silahkan login, update profile dan ganti password.",
                EmailInfoTitle="Kesalahan Ditemukan dalam Pemeriksaan Data",
                EmailInfoBody="Kesalahan ditemukan dalam pemeriksaan data untuk PSN: {dataTrack.TrackPSN}. Silakan periksa detail di sistem.",
            };

            // Menambahkan DataLine ke konteks
            context.WebConfigDatas.Add(webData);
            context.SaveChanges();

        }
    }
}