using Frac.Scenes;
using Frac.ViewModel;
using Orbit.Engine;

namespace Frac.Pages;

public partial class GamePage : ContentPage
{
    private readonly IGameSceneManager _gameSceneManager;
    private readonly GamePageViewModel _gamePageViewModel;
    private readonly IDeviceDisplay _deviceDisplay;
    public GamePage(
        IGameSceneManager gameSceneManager,
        GamePageViewModel drawingManager,
        IDeviceDisplay deviceDisplay)
    {
        InitializeComponent();

        BindingContext = drawingManager;

        _gameSceneManager = gameSceneManager;
        _gamePageViewModel = drawingManager;
        _deviceDisplay = deviceDisplay;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        _gameSceneManager.LoadScene<MainScene>(SceneView);
        _gameSceneManager.Start();
        _gamePageViewModel.Start();
    }
}
