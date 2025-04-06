using Rankomizer.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rankomizer.Mobile.ViewModels;



public class DuelViewModel : INotifyPropertyChanged
{
    private bool _isSubmitting;
    public bool IsSubmitting
    {
        get => _isSubmitting;
        set
        {
            if( _isSubmitting != value )
            {
                _isSubmitting = value;
                Notify();
            }
        }
    }

    public string DuelLeftTitle
    {
        get
        {
            if( DuelLeft is null ) return "";
            var movieDetails = DuelLeft.GetTypedDetails<MovieDetails>();

            if( movieDetails != null )
            {
                return $"{DuelLeft.Name} ({movieDetails.ReleaseDate.Year})";
            }
            else
            {
                return DuelLeft.Name;
            }
        }
    }
    public string DuelRightTitle
    {
        get
        {
            if(DuelRight is null) return "";
            var movieDetails = DuelRight.GetTypedDetails<MovieDetails>();

            if( movieDetails != null )
            {
                return $"{DuelRight.Name} ({movieDetails.ReleaseDate.Year})";
            }
            else
            {
                return DuelRight.Name;
            }
        }
    }

    public RosterItemDto DuelLeft { get; set; }
    public RosterItemDto DuelRight { get; set; }

    public ICommand LeftTapGesture { get; set; }
    public ICommand RightTapGesture { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void SetDuel( DuelDto duel, Func<Guid, Task> onPick )
    {
        bool flip = new Random().Next( 2 ) == 0;
        var left = flip ? duel.OptionA : duel.OptionB;
        var right = flip ? duel.OptionB : duel.OptionA;

        DuelLeft = left;
        DuelRight = right;

        LeftTapGesture = new Command( async () =>
        {
            if( IsSubmitting ) return;
            IsSubmitting = true;
            await onPick( left.Id );
            IsSubmitting = false;
        } );

        RightTapGesture = new Command( async () =>
        {
            if( IsSubmitting ) return;
            IsSubmitting = true;
            await onPick( right.Id );
            IsSubmitting = false;
        } );

        Notify( nameof( DuelLeft ) );
        Notify( nameof( DuelRight ) );
        Notify( nameof( LeftTapGesture ) );
        Notify( nameof( RightTapGesture ) );
        Notify( nameof( DuelLeftTitle ) );
        Notify( nameof( DuelRightTitle ) );
    }

    private void Notify( [CallerMemberName] string? propName = null )
    {
        PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propName ) );
    }
}

