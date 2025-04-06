using Rankomizer.Domain.Catalog;
using Rankomizer.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rankomizer.Mobile.Services;




public class GauntletApiService
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOptions;

    public GauntletApiService( HttpClient httpClient, JsonSerializerOptions jsonOptions )
    {
        _http = httpClient;
        _http.BaseAddress = new Uri( "http://192.168.1.217:5000/api/gauntlet/" );
        _jsonOptions = jsonOptions;
    }

    public async Task<DuelResponseDto?> StartGauntletAsync( Guid gauntletId )
    {
        try
        {
            var response = await _http.PostAsync( $"{gauntletId}/start", null );

            if( !response.IsSuccessStatusCode )
                return null;
            var text = await response.Content.ReadAsStringAsync();
            var d = JsonSerializer.Deserialize<DuelResponseDto>( text, _jsonOptions );
            var duelResponseDto = await response.Content.ReadFromJsonAsync<DuelResponseDto>( _jsonOptions );
            return duelResponseDto;
        }
        catch( Exception e )
        {
            Console.WriteLine( e.Message );
        }
        return null;
    }

    public async Task<DuelResponseDto?> SubmitDuelAsync( SubmitDuelRequest request )
    {
        var response = await _http.PostAsJsonAsync( "duel/submit", request );

        if( !response.IsSuccessStatusCode )
            return null;

        return await response.Content.ReadFromJsonAsync<DuelResponseDto>();
    }
}
