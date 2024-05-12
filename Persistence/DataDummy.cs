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


        }
    }
}