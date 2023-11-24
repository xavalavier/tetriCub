using Frac.ViewModel;

namespace Frac.Pages;

public partial class EndGamePage : ContentPage
{
	public EndGamePage(EndGamePageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}