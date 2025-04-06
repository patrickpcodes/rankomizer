using Rankomizer.Domain.DTOs;
using Rankomizer.Mobile.ViewModels;
using System.Windows.Input;

namespace Rankomizer.Mobile.Views;

public partial class DuelView : ContentView
{
    public DuelViewModel ViewModel => BindingContext as DuelViewModel;

    public DuelView()
    {
        InitializeComponent();
        BindingContext = new DuelViewModel(); // Could also bind from parent page
    }

    public void SetDuel( DuelDto duel, Func<Guid, Task> onPick )
    {
        ViewModel.SetDuel( duel, onPick );
    }
}