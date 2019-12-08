using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlannerRepository;
using TimeTrackerAPI.Configuration;
using Utilities;

namespace TimeTrackerAPI
{
    public class Startup
    {
        private readonly EventLogger eventlogger = new EventLogger(typeof(Startup));

        /// <summary>
        /// Statup constructor
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        public Startup(IConfiguration configuration)
        {
            eventlogger.LogInfo("Booting UP");
            SharedUtilities.Utility.SetConfiguration(null, configuration);
        }


        /// <summary>
        /// This method gets called by the runtime.
        /// Use this method to add services to the container.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            eventlogger.LogInfo("Started working for ConfigureServices.");

            services.AddCors(options =>
            {
                options.AddPolicy("VueCorsPolicy", builder =>
                {
                    builder
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .WithOrigins("http://localhost:8080");
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            DALConfiguration.ConfigureServices(services);

            services.AddAutoMapper(typeof(MappingProfileConfiguration).Assembly);
            services.AddSwagger();
            eventlogger.LogInfo("Done working with ConfigureServices.");
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the 
        /// HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IHostingEnvironment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCors("VueCorsPolicy");
            app.UseStaticFiles();
            app.UseSwaggerWithOptions();
            app.UseHttpsRedirection();
            app.UseMvc();
            eventlogger.LogInfo("Starting web server :------...");
        }
    }
}
