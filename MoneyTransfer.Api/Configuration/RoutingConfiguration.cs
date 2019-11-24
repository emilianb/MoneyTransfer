using Akka.Actor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

using MoneyTransfer.Api.Services;

namespace MoneyTransfer.Api.Configuration
{
    public static class RoutingConfiguration
    {
        public static IApplicationBuilder UseRouting(this IApplicationBuilder applicationBuilder)
        {
            if (applicationBuilder == null)
            {
                throw new ArgumentNullException(nameof(applicationBuilder), $"{nameof(applicationBuilder)} should not be null.");
            }

            var routes = new RouteBuilder(applicationBuilder);

            routes.MapGet("/greetings", async context =>
            {
                var name = context.Request.Query["name"].ToString();

                var greetingManagerActorProvider = (GreetingManagerActorProvider)context
                    .RequestServices
                    .GetService(typeof(GreetingManagerActorProvider));

                var greetingManagerActorRef = greetingManagerActorProvider();

                var greeting = await greetingManagerActorRef.Ask<string>(name, TimeSpan.FromSeconds(5));

                await context.Response.WriteAsync(greeting);
            });

            var router = routes.Build();

            applicationBuilder.UseRouter(router);

            return applicationBuilder;
        }
    }
}
