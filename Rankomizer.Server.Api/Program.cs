using System.Reflection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Rankomizer.Application;
using Rankomizer.Application.Catalog;
using Rankomizer.Application.Gauntlet;
using Rankomizer.Domain.Catalog;
using Rankomizer.Infrastructure;
using Rankomizer.Infrastructure.Catalog;
using Rankomizer.Infrastructure.Gauntlets;
using Rankomizer.Server.Api;
using Rankomizer.Server.Api.Extensions;
using Rankomizer.Server.Api.Seeding;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddSwaggerGenWithAuth();

builder.Services
       .AddApplication()
       .AddPresentation()
       .AddInfrastructure(builder.Configuration);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();
builder.Services.AddScoped<IGauntletService, GauntletService>();
builder.Services.AddCors( options =>
{
    options.AddPolicy( "MyCorsPolicy", policy =>
    {
        policy.WithOrigins( "http://localhost:3000" ) // Explicitly specify your Next.js origin
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // This is important when credentials are involved
    } );
} );
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000); // HTTP
    options.ListenAnyIP(7135, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

WebApplication app = builder.Build();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();

    app.ApplyMigrations();
}

// app.MapHealthChecks("health", new HealthCheckOptions
// {
//     ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
// });

// app.UseRequestContextLogging();
//
// app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();
app.UseCors( "MyCorsPolicy" );
// REMARK: If you want to use Controllers, you'll need this.
app.MapControllers();

SeedApplication.SeedWebApplication( app, builder.Configuration );

await app.RunAsync();

namespace Rankomizer.Server.Api
{
    public partial class Program;
}