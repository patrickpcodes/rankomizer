using Rankomizer.Domain.DTOs;

namespace Rankomizer.Mobile.Views;

public class GauntletPage : ContentPage
{
    public DuelView DuelView { get; private set; } = new();
    public RosterListView RosterListView { get; private set; } = new();

    public GauntletPage()
    {
        Title = "Gauntlet";

        if( DeviceInfo.Idiom == DeviceIdiom.Phone )
        {
            var tabbed = new TabbedPage();
            tabbed.Children.Add( new ContentPage { Title = "Duel", Content = DuelView } );
            tabbed.Children.Add( new ContentPage { Title = "Roster", Content = RosterListView } );

            Application.Current.MainPage = tabbed; // Or Navigation.PushAsync(tabbed) if not replacing MainPage
        }
        else
        {
            var layout = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                ColumnSpacing = 10,
                Padding = 10
            };
            layout.SetColumn( DuelView, 0 );
            layout.SetRow( DuelView, 0 );

            layout.SetColumn( RosterListView, 1 );
            layout.SetRow( RosterListView, 0 );

            layout.Children.Add( DuelView );
            layout.Children.Add( RosterListView );

            Content = layout;

            
        }
    }

    public void SetDuel( DuelDto duel, Func<Guid, Task> onPick )
        => DuelView.SetDuel( duel, onPick );

    public void SetRoster( IEnumerable<RosterItemDto> roster )
        => RosterListView.SetRoster( roster );
}