using Rankomizer.Mobile.Models;
using Rankomizer.Mobile.PageModels;

namespace Rankomizer.Mobile.Pages;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}