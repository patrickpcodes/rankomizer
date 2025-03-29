using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Rankomizer.Application.Abstractions.Authentication;
using Rankomizer.Application.Abstractions.Data;
using Rankomizer.Domain.Abstractions;
using Rankomizer.Domain.User;
using Rankomizer.Infrastructure.Authentication;
using Rankomizer.Infrastructure.Database;
using Rankomizer.Infrastructure.Time;

namespace Rankomizer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration ) =>
        services
            .AddServices()
            .AddDatabase( configuration )
            .AddHealthChecks( configuration )
            .AddAuthenticationInternal( configuration )
            .AddAuthorizationInternal();

    private static IServiceCollection AddServices( this IServiceCollection services )
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    private static IServiceCollection AddDatabase( this IServiceCollection services, IConfiguration configuration )
    {
        string? connectionString = configuration.GetConnectionString( "PostgresDB" );

        services.AddDbContext<ApplicationDbContext>(
            options => options
                       .UseNpgsql( connectionString, npgsqlOptions =>
                           npgsqlOptions.MigrationsHistoryTable( HistoryRepository.DefaultTableName, Schemas.Default ) )
                       .UseSnakeCaseNamingConvention() );

        // services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddIdentityCore<ApplicationUser>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        return services;
    }

    private static IServiceCollection AddHealthChecks( this IServiceCollection services, IConfiguration configuration )
    {
        services
            .AddHealthChecks()
            .AddNpgSql( configuration.GetConnectionString( "PostgresDB" )! );

        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(
        this IServiceCollection services,
        IConfiguration configuration )
    {
        services.AddAuthentication( ( options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                } ) )
                .AddJwtBearer( o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey =
                            new SymmetricSecurityKey( Encoding.UTF8.GetBytes( configuration["Jwt:Secret"]! ) ),
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero
                    };
                    o.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if ( context.Request.Cookies.ContainsKey( "rankomizer-jwt" ) )
                            {
                                context.Token = context.Request.Cookies["rankomizer-jwt"];
                            }

                            return Task.CompletedTask;
                        }
                    };
                } );

        services.AddHttpContextAccessor();
        // services.AddScoped<IUserContext, UserContext>();
        // services.AddSingleton<IPasswordHasher<>, PasswordHasher>();
        services.AddSingleton<ITokenProvider, TokenProvider>();

        return services;
    }

    private static IServiceCollection AddAuthorizationInternal( this IServiceCollection services )
    {
        services.AddAuthorization();

        // services.AddScoped<PermissionProvider>();
        //
        // services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        //
        // services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }
}