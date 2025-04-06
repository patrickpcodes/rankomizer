using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Rankomizer.Server.Api.Infrastructure;

namespace Rankomizer.Server.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // REMARK: If you want to use Controllers, you'll need this.
        services.AddControllers().AddJsonOptions(options =>
        {
            // Enums are returned as strings
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            // options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            // options.JsonSerializerOptions.IncludeFields = true;
            // options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
        });;
        services.Configure<JsonOptions>(options =>
        {
            // options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            // options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
        });
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}