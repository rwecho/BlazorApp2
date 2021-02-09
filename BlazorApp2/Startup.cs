using BlazorApp2.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp2.Pages;
using Microsoft.AspNetCore.Http;

namespace BlazorApp2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<HelloService>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<Middleware1>();
            app.UseMiddleware<Middleware2>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }

    public class Middleware2
    {
        private readonly RequestDelegate _next;
        private IServiceProvider ServiceProvider { get; }

        public Middleware2(IServiceProvider serviceProvider, RequestDelegate next)
        {
            _next = next;
            this.ServiceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Debug.WriteLine($"{nameof(Middleware2)} ->"+ this.ServiceProvider.GetHashCode());
            await _next(context);
        }
    }

    public class Middleware1
    {
        private readonly RequestDelegate _next;
        private IServiceProvider ServiceProvider { get; }

        public Middleware1(IServiceProvider serviceProvider, RequestDelegate next)
        {
            _next = next;
            this.ServiceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Debug.WriteLine($"{nameof(Middleware1)} ->" + this.ServiceProvider.GetHashCode());
            await _next(context);
        }
    }
}
