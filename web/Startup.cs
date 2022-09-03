using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

namespace web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

           app.Run(async (context) =>
            {
                 string apiMsg = await CallApi();
               
                string msg = $"Hello from the front end and the backend said {apiMsg}";
                await context.Response.WriteAsync(msg);
            });
        }

        private async Task<string> CallApi()
        {
            string msg = string.Empty;
             using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://backend/api/");
                //HTTP GET
                var result = await client.GetAsync("values");
               
              
                if (result.IsSuccessStatusCode)
                {
                   msg = await result.Content.ReadAsStringAsync();
                }
                else
                {
                    msg = "Error :'(";

                }
            }

            return msg;

        }
    }
}
