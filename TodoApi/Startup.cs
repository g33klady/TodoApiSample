using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.Models;
using NSwag.AspNetCore;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using TodoApi.Filters;
using NSwag.SwaggerGeneration.Processors.Security;
using NSwag;

namespace TodoApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TodoContext>(opt =>
                opt.UseInMemoryDatabase("TodoList"));
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
                    

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSwaggerUi3(typeof(Startup).GetTypeInfo().Assembly, settings =>
            {
                settings.GeneratorSettings.IsAspNetCore = true;
                settings.GeneratorSettings.OperationProcessors.Add(new OperationSecurityScopeProcessor("custom-auth"));

                settings.GeneratorSettings.DocumentProcessors.Add(
                    new SecurityDefinitionAppender("custom-auth", new SwaggerSecurityScheme
                    {
                        Type = SwaggerSecuritySchemeType.ApiKey,
                        Name = "CanAccess",
                        Description = "CanAccess Header",
                        In = SwaggerSecurityApiKeyLocation.Header
                    }));
            });
        app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, settings =>
            {
                
                settings.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "ToDo API";
                    document.Info.Description = "A simple ASP.NET Core web API";
                    document.Info.TermsOfService = "None";

                };
            });

            app.UseMvc();
        }
    }
}