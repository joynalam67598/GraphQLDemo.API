using GraphQLDemo.API.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Changes to applying migation on Startup.

            // we need a host first.
            IHost host = CreateHostBuilder(args).Build();

            // as our db context registerd as scoped we need to create a scop.
            using (IServiceScope scope = host.Services.CreateScope())
            {
                // we are gonna required a service which is SchoolDBContex.
                IDbContextFactory<SchoolDBContext> contextFactory =
                    scope.ServiceProvider.GetRequiredService<IDbContextFactory<SchoolDBContext>>();

                //create db context.
                using (SchoolDBContext schoolDBContext = contextFactory.CreateDbContext())
                {
                    //take data base and migrate.
                    schoolDBContext.Database.Migrate();
                }
            }
            // run host
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
