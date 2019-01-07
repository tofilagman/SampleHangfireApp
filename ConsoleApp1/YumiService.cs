using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace ConsoleApp1
{
    public class YumiService : WebHostService
    {
        public YumiService(IWebHost host) : base(host)
        {
        }

        protected override void OnStarting(string[] args)
        {
            //Process.Start($"netsh", $"http add urlacl url=http://*:{ ConfigManager.Conf.Port }/ user=everyone");
            base.OnStarting(args);
        }

        protected override void OnStarted()
        {
            base.OnStarted();
        }

        protected override void OnStopping()
        {
            base.OnStopping();
        }
    }

    public static class InSysHostServiceExtensions
    {
        public static void RunAsYumiService(this IWebHost host)
        {
            var webHostService = new YumiService(host);
            ServiceBase.Run(webHostService);
        }
    }
}
