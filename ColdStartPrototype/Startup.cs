using System;
using System.Diagnostics;
using ExtensionPrototype;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace ColdStartPrototype
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseWhen(context => Environment.GetEnvironmentVariable("PLACEHOLDERMODE") == "true", app =>
            {
                app.UseMiddleware<ColdStartDiagnosticsMiddleware>();
            });

            app.UseWhen(context => context.Request.Path.Value.EndsWith("specialize"), app =>
            {
                app.Use((context, next) =>
                {
                    // Fake specialization
                    Environment.SetEnvironmentVariable("PLACEHOLDERMODE", null);

                    _ = new HostBuilder()
                    .ConfigureServices(s =>
                    {
                        s.AddApplicationInsightsExtension();
                    })
                    .Build();

                    return next();
                });
            });

            app.Run(async context =>
            {
                Activity activity = Activity.Current;
                activity.AddTag("abc", "123");

                await context.Response.WriteAsync("Hello, World!");
            });
        }
    }
}
