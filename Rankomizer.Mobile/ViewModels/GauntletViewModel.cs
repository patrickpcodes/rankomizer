using Rankomizer.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rankomizer.Mobile.ViewModels;

public class GauntletViewModel : INotifyPropertyChanged
{
    private readonly GauntletApiService _api;

    public DuelDto? CurrentDuel { get; private set; }

    public ObservableCollection<RosterItemDto> Roster { get; private set; } = new();

    public bool IsLoading { get; private set; } = true;
    public bool IsFinished { get; private set; } = false;
    public bool IsSubmitting { get; private set; } = false;

    private bool _duelOrderFlipped = false;
    public bool DuelOrderFlipped => _duelOrderFlipped;

    public event PropertyChangedEventHandler? PropertyChanged;

    public GauntletViewModel( GauntletApiService api )
    {
        _api = api;
    }

    public async Task LoadFirstDuelAsync( Guid gauntletId )
    {
        IsLoading = true;
        Notify( nameof( IsLoading ) );

        var result = await _api.StartGauntletAsync( gauntletId );

        ProcessResponse( result );

        IsLoading = false;
        Notify( nameof( IsLoading ) );
    }

    public async Task SubmitDuelAsync( Guid winnerId )
    {
        if( CurrentDuel == null ) return;

        IsSubmitting = true;
        Notify( nameof( IsSubmitting ) );

        var request = new SubmitDuelRequest
        {
            DuelId = CurrentDuel.DuelId,
            WinnerRosterItemId = winnerId
        };

        var result = await _api.SubmitDuelAsync( request );

        ProcessResponse( result );

        IsSubmitting = false;
        Notify( nameof( IsSubmitting ) );
    }

    private void ProcessResponse( DuelResponseDto? response )
    {
        if( response == null )
        {
            IsFinished = true;
            Notify( nameof( IsFinished ) );
            return;
        }

        CurrentDuel = response.Duel;
        Notify( nameof( CurrentDuel ) );

        _duelOrderFlipped = new Random().Next( 2 ) == 0;
        Notify( nameof( DuelOrderFlipped ) );

        Roster.Clear();
        foreach( var item in response.Roster.OrderByDescending( r => r.Score ) )
        {
            Roster.Add( item );
        }
    }

    private void Notify( [CallerMemberName] string? propertyName = null )
    {
        PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
    }
}
