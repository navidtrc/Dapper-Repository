using DAL.Dapper;
using DAL.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebFramework.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CrmMainDb"));
            });

            services.AddScoped<DapperContext>();
        }

        public static void AddMinimalMvc(this IServiceCollection services)
        {
            //https://github.com/aspnet/AspNetCore/blob/0303c9e90b5b48b309a78c2ec9911db1812e6bf3/src/Mvc/Mvc/src/MvcServiceCollectionExtensions.cs
            services.AddControllers()
                .AddNewtonsoftJson(option =>
                {
                    option.SerializerSettings.Converters.Add(new StringEnumConverter());
                    option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //option.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                    //option.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });
            services.AddSwaggerGenNewtonsoftSupport();
        }

        public static void AddCustomApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true; //default => false;
                options.DefaultApiVersion = new ApiVersion(1, 0); //v1.0 == v1
                options.ReportApiVersions = true;
            });
        }

        //public static void AddElmahCore(this IServiceCollection services, IConfiguration configuration, SiteSettings siteSetting)
        //{
        //    services.AddElmah<SqlErrorLog>(options =>
        //    {
        //        options.Path = siteSetting.ElmahPath;
        //        options.ConnectionString = configuration.GetConnectionString("Elmah");
        //        //options.CheckPermissionAction = httpContext => httpContext.User.Identity.IsAuthenticated;
        //    });
        //}
    }
}
