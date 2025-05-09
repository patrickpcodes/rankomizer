using System.Text.Json;

namespace Rankomizer.Infrastructure.Json;

public static class JsonOptions
{
    public static readonly JsonSerializerOptions CamelCase = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}

public static class CustomParse<T> where T : class
{
    public static T Parse( string jsonString )
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        return JsonSerializer.Deserialize<T>( jsonString, options );
    }
}