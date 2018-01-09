using System.IO;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;

namespace BalanceWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = BuildWebHost(args);

            webHost.Run();
        }

        public static IWebHost BuildWebHost(string [] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options => {
                    options.Listen(IPAddress.Any, 5000);
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

    }
}
