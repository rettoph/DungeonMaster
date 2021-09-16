using DungeonMaster.Library;
using DungeonMaster.WebServer.Middleware;
using DungeonMaster.WebServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DungeonMaster.WebServer
{
    public class Startup
    {
        IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            DungeonSetup.ConfigureServices(
                services, 
                this.Configuration["ConnectionString"], 
                this.Configuration["Binaries:ffmpeg"], 
                this.Configuration["Binaries:youtube-dl"]);

            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<DiscordUser>();

            services.AddRouting();

            services.AddAuthentication(options => {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/signin";
                options.LogoutPath = "/signout";
            })
            .AddDiscord(options =>
            {
                options.ClientId = this.Configuration["Discord:ApplicationId"];
                options.ClientSecret = this.Configuration["Discord:OAuth2:ClientSecret"];

                options.Scope.Add("guilds");

                options.SaveTokens = true;

                options.Events.OnCreatingTicket = ctx =>
                {
                    var tokens = ctx.Properties.GetTokens().ToList();
                    ctx.Properties.StoreTokens(tokens);
                    return Task.CompletedTask;
                };
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<SetCurrentGuildMiddleware>();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            
            // RouteBuilder builder = new RouteBuilder(app);
            // 
            // builder.MapVerb(
            //     HttpMethod.Get.Method,
            //     "Guilds/{currentGuildId}",
            //     async context =>
            //     {
            // 
            //     });
            // 
            // app.UseRouter(builder.Build());
        }
    }
}
