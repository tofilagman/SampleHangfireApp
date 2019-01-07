using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class SampleJobTask
    {
        ISQLAdapter adp;

        public SampleJobTask(ISQLAdapter adp)
        {
            this.adp = adp;
        }

        [DisplayName("Runway"), AutomaticRetry(Attempts = 0)]
        public void RunWay(string text, PerformContext context)
        {
            try
            {
                context.WriteLine(text);

                var df = adp.GetData("Select * from ttesttable");

                context.WriteLine($"Data Count: {df.Rows.Count}");

                context.WriteLine($"Data Count: {df.Rows.Count}");

                context.WriteLine($"Data Count: {df.Rows.Count}");

                context.WriteLine($"Data Count: {df.Rows.Count}");

                //throw new InvalidOperationException("Some invalid ");

                var bar = context.WriteProgressBar();
                for(var i =0; i<= 100; i++)
                {
                    bar.SetValue(i);
                    Task.Delay(new Random().Next(500, 2000)).Wait();
                }


            }catch(Exception ex)
            {
                context.WriteLine(ex.Message);
                throw ex;
            }
        }

    }
}
