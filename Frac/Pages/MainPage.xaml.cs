using Frac.ViewModel;
using Microsoft.Maui.Controls;
using System.Windows;
using System.Windows.Input;

namespace Frac.Pages;

public partial class MainPage : ContentPage
{
    private readonly List<Button> _levelButtons = new List<Button>();
    private readonly List<Button> _layerButtons = new List<Button>();
    private readonly MainPageViewModel _mainPageViewModel;
    public MainPage(MainPageViewModel vm)
	{
        InitializeComponent();
        CreateLevelButtons();
        CreateLayerButtons();
        BindingContext = vm;
        _mainPageViewModel =vm;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing(); 
        if(_mainPageViewModel.End == "True")
            Shell.Current.GoToAsync(nameof(EndGamePage));

    }

    private void CreateLevelButtons()
    {

        for (var i=1; i < 10; i++)
        {
            var button = new Button
            {
                Text = $"{i}",
                CommandParameter = i
            };
            if (i == 1)
                button.BackgroundColor = Colors.LawnGreen;
            button.Clicked += ClickedLevelButton;
            button.SetBinding(Button.CommandProperty, "SelectLevelCommand");
            ButtonGrid.Add(button, (i-1)%3, (i-1)/3 +2);
            _levelButtons.Add(button);
        }
    }
    private void CreateLayerButtons()
    {

        for (var i=0; i < 9; i++)
        {
            var button = new Button
            {
                Text = $"{i}",
                CommandParameter = i
            };
            button.Clicked += ClickedLayerButton;
            if (i == 0)
                button.BackgroundColor = Colors.LawnGreen;
            button.SetBinding(Button.CommandProperty, "SelectLayersCommand");
            ButtonGrid.Add(button, i%3 + 4, i/3 +2);
            _layerButtons.Add(button);
        }
    }

    private void ClickedLevelButton(object sender, EventArgs ar)
    {
        foreach (var  button in _levelButtons)
        {
            button.BackgroundColor = Colors.DarkGray;
        }
        var btn = (Button)sender;
        btn.BackgroundColor = Colors.LawnGreen;
    }
    private void ClickedLayerButton(object sender, EventArgs ar)
    {
        foreach (var  button in _layerButtons)
        {
            button.BackgroundColor = Colors.DarkGray;
        }
        var btn = (Button)sender;
        btn.BackgroundColor = Colors.LawnGreen;
    }
}