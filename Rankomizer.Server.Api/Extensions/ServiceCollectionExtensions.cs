using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Rankomizer.Server.Api.Extensions;

public static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddSwaggerGenWithAuth( this IServiceCollection services )
    {
        services.AddSwaggerGen( o =>
        {
            o.CustomSchemaIds( id => id.FullName!.Replace( '+', '-' ) );

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter your JWT token in this field",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            };

            o.AddSecurityDefinition( JwtBearerDefaults.AuthenticationScheme, securityScheme );

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    []
                }
            };

            o.AddSecurityRequirement( securityRequirement );
        } );

        return services;
    }


    public static void RemoveDbContext<T>( this IServiceCollection services ) where T : DbContext
    {
        var descriptor = services.SingleOrDefault( d => d.ServiceType == typeof(DbContextOptions<T>) );
        if ( descriptor != null ) services.Remove( descriptor );
    }

    public static void EnsureDbCreated<T>( this IServiceCollection services ) where T : DbContext
    {
        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<T>();
        context.Database.EnsureCreated();
    }
}