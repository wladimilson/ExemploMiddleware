using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ExemploMiddleware
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Primeiro middleware (antes)");
                await next();
                await context.Response.WriteAsync("Primeiro middleware (depois)");
            });
        
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Segundo middleware (antes)");
                await next();
                await context.Response.WriteAsync("Segundo middleware (depois)");
            });

            app.Map("/foo", 
                (a) => {
                    a.Use(async (context, next) => {
                        await context.Response.WriteAsync("Middleware para o caminho /foo (antes) ");
                        await next();
                        await context.Response.WriteAsync("Middleware para o caminho /foo (depois) ");
                    });
            });

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/bar"), 
                (a) => {
                    a.Use(async (context, next) => {
                        await context.Response.WriteAsync("Middleware para o caminho /bar (antes) ");
                        await next();
                        await context.Response.WriteAsync("Middleware para o caminho /bar (depois) ");
                    });
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Middleware final");
            });
        }
    }
}
