using Frac.Scenes;
using Frac.ViewModel;
using Orbit.Engine;

namespace Frac.Pages;

public partial class GamePage : ContentPage
{
    private readonly IGameSceneManager _gameSceneManager;
    private readonly GamePageViewModel _gamePageViewModel;
    public GamePage(
        IGameSceneManager gameSceneManager,
        GamePageViewModel drawingManager
    )
    {
        InitializeComponent();

        BindingContext = drawingManager;

        _gameSceneManager = gameSceneManager;
        _gamePageViewModel = drawingManager;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        _gameSceneManager.LoadScene<MainScene>(SceneView);
        _gameSceneManager.Start();
        _gamePageViewModel.Start();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _gameSceneManager.GameOver();
    }
}
