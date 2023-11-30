using Frac.Scenes;
using Frac.ViewModel;
using Orbit.Engine;

namespace Frac.Pages;

public partial class EndGamePage : ContentPage
{
    private readonly IGameSceneManager _gameSceneManager;
    private readonly EndGamePageViewModel _endGamePageViewModel;
    private readonly GamePageViewModel _gamePageViewModel;
    public EndGamePage(
        EndGamePageViewModel vm,
        IGameSceneManager GameSceneManager,
        GamePageViewModel GamePageViewModel)
    {
		InitializeComponent();
        BindingContext = vm;
        _gameSceneManager = GameSceneManager;
        _endGamePageViewModel = vm;
        _gamePageViewModel = GamePageViewModel;

        CreateParts();
    }

    private void CreateParts()
    {
        for (int i = 0; i < 9; i++) 
        {
            var label = new Label
            {
                Text = $": {_gamePageViewModel.CubeFallen[i]}"
            };
            LabelGrid.Add(label,i%3 + 1, i/3);

        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _gameSceneManager.LoadScene<EndScene>(SceneView);
        _gameSceneManager.Start();
        _endGamePageViewModel.Start(_gamePageViewModel);
    } 
}