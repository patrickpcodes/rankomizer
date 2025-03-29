using System.Reflection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Rankomizer.Application;
using Rankomizer.Application.Catalog;
using Rankomizer.Infrastructure;
using Rankomizer.Infrastructure.Catalog;
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

// REMARK: If you want to use Controllers, you'll need this.
app.MapControllers();

SeedApplication.SeedWebApplication( app );

await app.RunAsync();

namespace Rankomizer.Server.Api
{
    public partial class Program;
}