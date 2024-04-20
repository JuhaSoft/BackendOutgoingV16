using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
             var host=CreateHostBuilder(args).Build();
            // CreateHostBuilder(args).Build().Run();
            using var scope = host.Services.CreateScope();
            var services=scope.ServiceProvider;
            try
            {
                var userManager=services.GetRequiredService<UserManager<AppUser>>();
               var context=services.GetRequiredService<DataContext>();
               await context.Database.MigrateAsync();
await DataDummy.SeedAllData(context,userManager);
            // // // //    context.Database.Migrate();
            // // //    await Seed.SeedLineData(context);
            // // // //    await Seed.SeedReferenceData(context);
            // // //    await Seed.SeedPCData(context);
            // // // //    await Seed.SeedLastStationData(context);
            // // //    await Seed.SeedUserData(context);
              
            // // //     await Seed.SeedDataType(context);
            // // //     await Seed.SeedParameterCheckData(context);
            // // //     // await Seed.SeedDTCheckData(context);
            // // //     // await Seed.SeedDTCheckImageData(context);
                //  await Seed.SeedDatatrackData(context);
            }
            catch (System.Exception ex)
            {
                var logger= services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex,"An Error Ocured during Migration");
                 // TODO
            }
           await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
