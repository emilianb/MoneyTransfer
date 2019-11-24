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
                .AddMapping()
                .AddWebApi(builder => builder.AddValidation())
                .AddMoneyTransfer();

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
            => app
                .UseGlobalExceptionHandler()
                .UseHttpsRedirection()
                .UseRouting()
                .UseMvc();
    }
}
