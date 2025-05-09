
using Rankomizer.Mobile.ViewModels;
using Rankomizer.Mobile.Views;

namespace Rankomizer.Mobile.Home;

public partial class HomePage : ContentPage
{
    private readonly GauntletViewModel _vm;

    // Use your actual GauntletId here (or wire up selection later)
    private readonly Guid _testGauntletId = Guid.Parse( "e449d035-fe7b-4840-b027-902c6867bb50" );

    public HomePage( GauntletViewModel viewModel )
    {
        InitializeComponent();
        _vm = viewModel;
    }

    private async void OnStartGauntletClicked( object sender, EventArgs e )
    {
        var gauntletPage = new GauntletPage();
        await _vm.LoadFirstDuelAsync( _testGauntletId );

        await SetupGauntletInteractionAsync( gauntletPage );

        await Navigation.PushAsync( gauntletPage );
    }

    private async Task SetupGauntletInteractionAsync( GauntletPage page )
    {
        async Task HandleNextDuel( Guid winnerId )
        {
            await _vm.SubmitDuelAsync( winnerId );
            page.SetDuel( _vm.CurrentDuel, HandleNextDuel );
            page.SetRoster( _vm.Roster );
        }

        page.SetDuel( _vm.CurrentDuel, HandleNextDuel );
        page.SetRoster( _vm.Roster );
    }

    //private async void OnStartGauntletClicked( object sender, EventArgs e )
    //{
    //    ResultLabel.Text = "Loading...";
    //    await _vm.LoadFirstDuelAsync( _testGauntletId );

    //    if( _vm.CurrentDuel != null )
    //    {
    //        ResultLabel.Text = $"Duel loaded: {_vm.CurrentDuel.OptionA.Name} vs {_vm.CurrentDuel.OptionB.Name}";
    //        await LoadAndDisplayNextDuel();
    //    }
    //    else if( _vm.IsFinished )
    //    {
    //        ResultLabel.Text = "No more duels — gauntlet is finished!";
    //    }
    //    else
    //    {
    //        ResultLabel.Text = "Failed to load duel.";
    //    }
    //}

    //private async Task LoadAndDisplayNextDuel()
    //{
    //    if( _vm.CurrentDuel != null )
    //    {
    //        DuelControl.SetDuel( _vm.CurrentDuel, async ( winnerId ) =>
    //        {
    //            await _vm.SubmitDuelAsync( winnerId );
    //            await LoadAndDisplayNextDuel();
    //        } );

    //        RosterControl.SetRoster( _vm.Roster );
    //    }
    //    else if( _vm.IsFinished )
    //    {
    //        RosterControl.SetRoster( _vm.Roster );
    //        await MainThread.InvokeOnMainThreadAsync( () =>
    //        {
    //            //Application.Current.MainPage.DisplayAlert( "Done", "You've completed the gauntlet!", "OK" );
    //        } );
    //    }
    //}

}