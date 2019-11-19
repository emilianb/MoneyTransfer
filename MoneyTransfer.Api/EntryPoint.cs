using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

using MoneyTransfer.Api.Configuration;

namespace MoneyTransfer.Api
{
    public static class EntryPoint
    {
        public static void Main(string[] args)
            => CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
            => WebHost
                .CreateDefaultBuilder(args)
                .ConfigureLogging()
                .UseStartup<Startup>();
    }
}
