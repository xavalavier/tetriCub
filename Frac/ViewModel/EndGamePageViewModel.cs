using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static Frac.DrawingHelper;

namespace Frac.ViewModel
{
    public partial class EndGamePageViewModel: ObservableObject
    {
        [ObservableProperty]
        private int _score;
        [ObservableProperty]
        private int _level;

        GamePageViewModel _gamePageViewModel;
        public IReadOnlyList<Cube> CubeToDraw => _cubesToDraw.ToList();
        private readonly List<Cube> _cubesToDraw = new List<Cube>();

        internal void Start(GamePageViewModel GamePageViewModel)
        {
            _gamePageViewModel = GamePageViewModel;
            Score = _gamePageViewModel.Score;
            Level = _gamePageViewModel.Level;
            for (int i = 0; i < GamePageViewModel.CubeFallen.Count; i++) 
            {
                var cubeData = CubeDictionnary[i];
                Part part = new(cubeData.Length, cubeData.Width, cubeData.Height, cubeData.Color);
                Cube.CreateCubes(EndGameXOffset + i%3*EndGameXFact, 0, EndGameYOffset + EndGameYFact * (i/3), ref part);
                _cubesToDraw.AddRange(part.Cubes);
            }
            _gamePageViewModel.End = false;
            _gamePageViewModel.CubeFallen = new List<int>();
        }

        [RelayCommand]
        internal void Back()
            => Shell.Current.GoToAsync($"..?End={_gamePageViewModel.End}");
    }
}
