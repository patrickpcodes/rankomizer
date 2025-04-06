using Microsoft.Maui.Controls.Shapes;
using Rankomizer.Domain.DTOs;

namespace Rankomizer.Mobile.Views;

public partial class RosterListView : ContentView
{
    public RosterListView()
    {
        InitializeComponent();
    }

    public void SetRoster( IEnumerable<RosterItemDto> items )
    {
        var sorted = items.OrderByDescending( i => i.Score ).ToList();
        RosterStack.Children.Clear();

        int rank = 1;
        foreach( var item in sorted )
        {
            var row = new HorizontalStackLayout
            {
                Spacing = 10,
                GestureRecognizers =
                {
                    new TapGestureRecognizer
                    {
                        Command = new Command(() => OnItemTapped(item))
                    }
                },
                Children =
                {
                    new Image
                    {
                        Source = item.ImageUrl,
                        HeightRequest = 40,
                        WidthRequest = 40,
                        Aspect = Aspect.AspectFill,
                        Clip = new RoundRectangleGeometry
                        {
                            CornerRadius = 8,
                            Rect = new Rect(0, 0, 40, 40)
                        }
                    },
                    new VerticalStackLayout
                    {
                        Spacing = 2,
                        Children =
                        {
                            new Label { Text = $"{rank}. {item.Name}", FontAttributes = FontAttributes.Bold, FontSize = 14 },
                            new Label { Text = $"W: {item.Wins}  L: {item.Losses}  • Score: {item.Score}", FontSize = 12, TextColor = Colors.Gray }
                        }
                    }
                },
                
            };

            RosterStack.Children.Add( row );
            rank++;
        }
    }

    private void OnItemTapped( RosterItemDto item )
    {
        Console.WriteLine( $"Clicked on item {item.Name}" );
    }
}