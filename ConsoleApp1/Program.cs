using Hangfire;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebHost builder = BuildWebHost();

            builder.Run(); //RunAsYumiService(); 
        }

        public static IWebHost BuildWebHost()
        {
            var host = new WebHostBuilder()
               .UseKestrel(x =>
               {
                   x.ListenAnyIP(2020);
               })
              .UseContentRoot(Directory.GetCurrentDirectory()) 
              .UseStartup<Startup>() 
              .Build();

            return host;
        }
    }
}
