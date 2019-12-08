using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PlannerRepository.Models;
using Swashbuckle.AspNetCore.Swagger;
using TimeTrackerAPI.DTO;

namespace TimeTrackerAPI.Configuration
{
    /// <summary>
    /// Custom Configurations
    /// </summary>
    public static class CustomConfiguration
    {
        public const string DateTimeFormat = "dd-MM-yyyy HH:mm:ss";
        public static string EndPoint => $"/swagger/{Version}/swagger.json";
        public static string ApiName => "Time Tracker API";
        public static string Version => "v1";

        /// <summary>
        /// Integrate Swagger into Application
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
                                    {
                                        c.SwaggerDoc(Version,
                                            new Info { Title = ApiName, Version = Version }
                                            );

                                        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                                        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                                        if (File.Exists(xmlPath))
                                        {
                                            c.IncludeXmlComments(xmlPath);
                                        }
                                    });

            return services;
        }

        /// <summary>
        /// Specify Swagger options
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerWithOptions(this IApplicationBuilder app)
        {
            SwaggerBuilderExtensions.UseSwagger(app);

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint(EndPoint, ApiName);
            });

            return app;
        }
    }

    /// <summary>
    /// Profile for Auto mapper
    /// </summary>
    public class MappingProfileConfiguration : Profile
    {
        /// <summary>
        /// Constructor with defined mapping
        /// </summary>
        public MappingProfileConfiguration()
        {
            CreateMap<Project, DTOProject>();
            CreateMap<Milestone, DTOMilestone>()
                .ForMember(dtoMilestone => dtoMilestone.StartDateTime,
                    expression => expression.MapFrom(milestone => milestone.StartDate))
                .ForMember(dtoMilestone => dtoMilestone.EndDateTime,
                    expression => expression.MapFrom(milestone => milestone.TargetDate))
                .ForMember(dtoMilestone => dtoMilestone.Title,
                    expression => expression.MapFrom(milestone => milestone.MilestoneDetail))
                .ForMember(dtoMilestone => dtoMilestone.ItemId,
                    expression => expression.MapFrom(milestone => milestone.Id));
        }
    }

}
