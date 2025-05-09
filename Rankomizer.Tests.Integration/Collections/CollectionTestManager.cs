using System;
using System.Text.Json;
using Rankomizer.Domain.DTOs;
using Rankomizer.Domain.Catalog;
using System.Threading.Tasks;
namespace Rankomizer.Tests.Integration.Collections;

public static class CollectionTestManager
{
    public static async Task<Guid> CreateCollectionAsync(
        HttpClient client,
        Collection collection,
        string rankomizerJwt)
    {
       
        // Use the JWT cookie in a future call by manually adding it to the request header
        var request = new HttpRequestMessage( HttpMethod.Post, "/api/collections" );
        request.Headers.Add( "Cookie", $"rankomizer-jwt={rankomizerJwt}" );
        request.Content = new StringContent( JsonSerializer.Serialize( collection ) );

        request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue( "application/json" );
        var authorizedResponse = await client.SendAsync( request );
        authorizedResponse.EnsureSuccessStatusCode();
        var result = await authorizedResponse.Content.ReadAsStringAsync();
        //Deserialize the result to CollectionDto in object collection
        using var doc = JsonDocument.Parse(result);
        var collectionId = doc.RootElement.GetProperty("collectionId").GetString();
        return Guid.Parse( collectionId );
        
        
    }
    public static async Task<CollectionDto> GetCollectionAsync(
        HttpClient client,
        string rankomizerJwt,
        Guid collectionId )
    {
        var request = new HttpRequestMessage( HttpMethod.Get, $"/api/collections/{collectionId}" );
        request.Headers.Add( "Cookie", $"rankomizer-jwt={rankomizerJwt}" );

        var authorizedResponse = await client.SendAsync( request );
        authorizedResponse.EnsureSuccessStatusCode();
        var result = await authorizedResponse.Content.ReadAsStringAsync();
        //Deserialize the result to CollectionDto in object collection
        JsonDocument doc = JsonDocument.Parse( result );
        JsonElement collectionElement = doc.RootElement.GetProperty( "collection" );
        var collectionDto = JsonSerializer.Deserialize<CollectionDto>( collectionElement.ToString() );
        return collectionDto;
    }
    
    
}