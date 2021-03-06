﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NewsFeed.Entites;

namespace NewsFeedAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=NewsDatabase;Trusted_Connection=True;";
            services.AddAutoMapper();

            //Gør repo tilgængeligt for app'ens andre gennem constructor injection
            //Sætter livstiden for servicen for et enkelt request
            services.AddScoped<INewsFeedRepo, NewsFeedRepo>();
            
            //Transient services er skabt de bliver requested fra servicecontaineren
            //Singelton services er skabt først gang de bliver requested, hvert request derefter bruger den samme instans
            //Scoped services er skabt en gang per client request(Http request)


            services.AddDbContext<NewsFeedContext>(cfg => {


                cfg.UseSqlServer(connectionString);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, NewsFeedContext newsFeed)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStatusCodePages();

            //newsFeed.Seed();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
