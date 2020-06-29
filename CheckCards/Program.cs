using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckCards.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CheckCards
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (IServiceScope scope = host.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                Task t = init(services);
                Task.WaitAll(new Task[] { t });
            }

            host.Run();
        }

        public async static Task init(IServiceProvider services)
        {
            await InitializeCheckCards.ResetDatabase(
                    services.GetRequiredService<ApplicationDbContext>(),
                    services.GetRequiredService<UserManager<ApplicationUser>>(),
                    services.GetRequiredService<RoleManager<IdentityRole>>());
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
