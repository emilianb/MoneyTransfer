using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

using MoneyTransfer.Api.Configuration;

namespace MoneyTransfer.Api
{
    public class Startup
    {
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
            => services
                .AddMappings()
                .AddWebApi(builder => builder.AddValidations())
                .AddMoneyTransferService();

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
            => app
                .UseExceptions()
                .UseHttpsRedirection()
                .UseRouting()
                .UseMvc();
    }
}
