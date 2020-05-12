using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace Modelo.API.Configuration
{
    public static class SwaggerConfig
    {
        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            // app.UseMiddleware<SwaggerAuthorizedMiddleware>();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "v1");
            });

            return app;
        }

        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(name: "v1", new OpenApiInfo
                {
                    Title = "Modelo de API Enterprise",
                    Description = "Está API faz parte do módulo de autenticação",
                    Contact = new OpenApiContact() { Name = "Anderson Peluso", Email = "anderson.peluso@outlook.com" },
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri(uriString: "https://opensource.org/licenses/MIT") }
                });

                // OpenAPI 3.0 e na RFC7235
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    // Scheme = "bearer",
                    // Type = SecuritySchemeType.Http,
                    //Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                    new OpenApiSecurityScheme{ Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }},new List<string>()
                }
                });
            });

            return services;
        }
    }

    public class SwaggerAuthorizedMiddleware
    {
        private readonly RequestDelegate Proximo;

        public SwaggerAuthorizedMiddleware(RequestDelegate next)
        {
            Proximo = next;
        }

        public async System.Threading.Tasks.Task Invoke(HttpContext contexto)
        {
            if (contexto.Request.Path.StartsWithSegments(other: "/swagger") && !contexto.User.Identity.IsAuthenticated)
            {
                contexto.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            await Proximo.Invoke(contexto);
        }
    }
}