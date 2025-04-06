
using Rankomizer.Mobile.ViewModels;

namespace Rankomizer.Mobile.Home;

public partial class HomePage : ContentPage
{
    private readonly GauntletViewModel _vm;

    // Use your actual GauntletId here (or wire up selection later)
    private readonly Guid _testGauntletId = Guid.Parse( "f8574163-30a6-4552-a10f-93c4e7be85a1" );

    public HomePage( GauntletViewModel viewModel )
    {
        InitializeComponent();
        _vm = viewModel;
    }

    private async void OnStartGauntletClicked( object sender, EventArgs e )
    {
        ResultLabel.Text = "Loading...";
        await _vm.LoadFirstDuelAsync( _testGauntletId );

        if( _vm.CurrentDuel != null )
        {
            ResultLabel.Text = $"Duel loaded: {_vm.CurrentDuel.OptionA.Name} vs {_vm.CurrentDuel.OptionB.Name}";
            await LoadAndDisplayNextDuel();
        }
        else if( _vm.IsFinished )
        {
            ResultLabel.Text = "No more duels — gauntlet is finished!";
        }
        else
        {
            ResultLabel.Text = "Failed to load duel.";
        }
    }

    private async Task LoadAndDisplayNextDuel()
    {
        if( _vm.CurrentDuel != null )
        {
            DuelControl.SetDuel( _vm.CurrentDuel, async ( winnerId ) =>
            {
                await _vm.SubmitDuelAsync( winnerId );
                await LoadAndDisplayNextDuel();
            } );

            RosterControl.SetRoster( _vm.Roster );
        }
        else if( _vm.IsFinished )
        {
            RosterControl.SetRoster( _vm.Roster );
            await MainThread.InvokeOnMainThreadAsync( () =>
            {
                //Application.Current.MainPage.DisplayAlert( "Done", "You've completed the gauntlet!", "OK" );
            } );
        }
    }

}