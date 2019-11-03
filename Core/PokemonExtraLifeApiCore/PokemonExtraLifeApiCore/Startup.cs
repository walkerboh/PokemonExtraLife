﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PokemonExtraLifeApiCore.Common;
using PokemonExtraLifeApiCore.EntityFramework;
using PokemonExtraLifeApiCore.EntityFramework.Initialization;
using PokemonExtraLifeApiCore.ExtraLife;
using PokemonExtraLifeApiCore.Models.API;
using Serilog;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace PokemonExtraLifeApiCore
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
            services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ExtraLifeContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("ExtraLifeDatabase")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<ExtraLifeApiSettings>(Configuration.GetSection("ExtraLifeSettings"));

            services.AddTransient<DonationProcessor>();
            services.AddTransient<IScopedProcessingService, ExtraLifeScopedService>();
            services.AddSingleton<IHostedService, ExtraLifeService>();
            services.AddTransient<Random>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddSingleton((ILogger) new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("logs.log")
                .CreateLogger());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ExtraLifeContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseCors();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            context.Database.EnsureCreated();

            // DB Initialization
            context.Pokemon.AddRange(PokemonInitialization.Pokemon.Where(pokemon => context.Pokemon.Find(pokemon.Id) == null));

            context.Trainers.AddRange(TrainerInitialization.Trainers.Where(trainer => context.Trainers.Find(trainer.Id) == null));

            context.Groups.AddRange(GroupInitialization.Groups.Where(group => context.Groups.Find(group.Id) == null));

            context.Hosts.AddRange(HostInitialization.Hosts.Where(host => context.Hosts.Find(host.Id) == null));

            context.Giveaways.AddRange(GiveawayInitialization.Giveaways.Where(giveaway => context.Giveaways.Find(giveaway.Id) == null));

            context.SaveChanges();

            context.PokemonOrders.AddRange(PokemonOrderInitialization.PokemonOrders.Where(po => context.PokemonOrders.Find(po.PokemonId) == null));

            context.Prizes.AddRange(PrizeInitialization.Prizes.Where(p => context.Prizes.Find(p.Id) == null));

            context.Facts.AddRange(FactInitialization.Facts.Where(f => context.Facts.Find(f.Id) == null));

            context.SaveChanges();
        }
    }
}
