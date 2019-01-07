using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using Hangfire.Console;
using System.Collections.Generic;
using System.Text;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.Heartbeat.Server;
using Hangfire.Heartbeat;
using System.Linq;
using Hangfire.SQLite;
using System.IO;

namespace ConsoleApp1
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            const string connString = "Server=192.168.100.12;Initial Catalog=TestHangfire;User ID=sa;Password=dev123sql$%^;";
            string sqlite = $"Data Source={ Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) }/Hangfire.sqlite;";

            SQLitePCL.Batteries.Init();
            services.AddHangfire(x =>
                {
                    //x.UseSqlServerStorage(connString);
                    x.UseSQLiteStorage(sqlite, new SQLiteStorageOptions
                    {
                        PrepareSchemaIfNecessary = true,
                        SchemaName = "Hangfire"
                    });
                    x.UseConsole();
                    x.UseHeartbeatPage(checkInterval: TimeSpan.FromSeconds(2));
                });

            services.AddSingleton<ISQLAdapter>(new SQLAdapter(connString));
            services.AddSingleton<SampleJobTask>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHangfireServer(additionalProcesses: new[] {
                new SystemMonitor(checkInterval: TimeSpan.FromSeconds(2))
            });

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[]
                {
                    new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions {
                         SslRedirect = false,
                          RequireSsl = false,
                           LoginCaseSensitive = false,
                           Users = new[] {
                               new BasicAuthAuthorizationUser
                               {
                                    Login = "Admin",
                                    PasswordClear = "123"
                               }
                           }
                    })
                },

            });

            //cleanup old servers
            var monitoringApi = JobStorage.Current.GetMonitoringApi();
            var servers = monitoringApi.Servers().ToArray();
            for (var ii = 1; ii < servers.Length; ii++)
                JobStorage.Current.GetConnection().RemoveServer(servers[ii].Name);

            BackgroundJob.Enqueue(() => Console.WriteLine("Hello, world!"));
            RecurringJob.AddOrUpdate<SampleJobTask>(x => x.RunWay("test", null), "1 * * * *");
        }
    }
}
