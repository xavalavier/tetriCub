using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using SharpHook;
using System.Reactive.Linq;
using System.Timers;
using static Frac.DrawingHelper;

namespace Frac.ViewModel;

public partial class GamePageViewModel : ObservableObject, IDisposable
{
    private int _xGrid;
    private int _yGrid;
    private int _altitude = 10;
    [ObservableProperty]
    private string _pauseText = "Pause";

    [ObservableProperty]
    private int _score = 0;
    private int _startingfib = 0;

    [ObservableProperty]
    private int _level = 1;

    [ObservableProperty]
    private int _clearedLayers = 0;

    [ObservableProperty]
    private TimeSpan _timePlayed;

    internal bool End = false;

    private List<Cube> _cubesToDraw = new List<Cube>();
    private readonly List<Cube> _cubesInPlace = new List<Cube>();
    private readonly List<Cube> _cubesToDelete = new List<Cube>();
    private int _deleteLineEffectCount = 0;
    private Part _fallingCubes;
    private Part _nextFallingCubes;
    private List<Cube> _fallingShadows = new List<Cube>();
    private System.Timers.Timer _gameTimer;
    private System.Timers.Timer _deleteLineTimer;
    private TaskPoolGlobalHook _hook;
    private readonly List<int> _cubeProbabilities = new List<int>() {-1, 15, 30, 43, 64, 73, 82, 89, 96};

    public IReadOnlyList<Cube> CubeToDraw => _cubesToDraw.ToList();
    public List<int> CubeFallen { get; set; } = new List<int>();

    public void Start()
    {
        _startingfib = 0;
        Score = 0;
        _hook = new TaskPoolGlobalHook();
        _hook.KeyPressed += OnKeyPressed;
        _hook.RunAsync();
        Level = StartingLevel;
        for (int i = 0; i < CubeDictionnary.Count; i++)
            CubeFallen.Add(0);
        for (var i = 1; i < Level; i++)
            _startingfib += 1000 * i;
        _nextFallingCubes = CreateNewFallingCube();
        NextPart();
        AddStartingLayer();
        UpdateFallingCube();
        _gameTimer = new System.Timers.Timer()
        {
            Interval = (10 - Level)* 100,
            AutoReset = true
        };
        
        _deleteLineTimer = new System.Timers.Timer()
        {
            Interval = 100,
            AutoReset = true
        };        

        _gameTimer.Elapsed += GameTimerTrigger;
        _deleteLineTimer.Elapsed += DeleteLineEffect;
        _gameTimer.Start();
    }


    private void AddStartingLayer()
    {
        var rand = new Random();
        for (var layer = 0; layer < StartingLayers; layer++)
            for (var x = 0;x < GridSizeX; x++ )
                for (var y = 0;y < GridSizeY; y++ )
                    if(rand.Next(2)  == 0)
                    {
                        _cubesInPlace.Add(Cube.CreateCube(x, y, layer,  SupportedColors[rand.Next(10) + 1]));
                    }
    }

    private void DeleteLineEffect(object sender, ElapsedEventArgs e)
    {

        if (_deleteLineEffectCount % 2 == 0)
            _cubesToDelete.ForEach(cube => cube.Color = Colors.White);
        else
            _cubesToDelete.ForEach(cube => cube.Color = _fallingCubes.Color);
        _deleteLineEffectCount++;
        if (_deleteLineEffectCount > 15)
        {
            _deleteLineTimer.Stop();
            _cubesInPlace.RemoveMany(_cubesToDelete);
            for (var z = _cubesToDelete.Last().Z; z >= 0; z--)
            {
                if (!_cubesInPlace.Where(cube => cube.Z == z).Any())
                {
                    var cubesToMove = _cubesInPlace.Where(cube => cube.Z > z);
                    var cubesToMoveList = cubesToMove.ToList();
                    _cubesInPlace.RemoveMany(cubesToMove);
                    foreach (var cube in cubesToMoveList)
                        _cubesInPlace.Add(Cube.CreateCube(cube.X, cube.Y, cube.Z - 1, cube.Color));
                }
            }
            _cubesToDelete.Clear();
            NextPart();

            UpdateFallingCube();
            _gameTimer.Start();
            _deleteLineEffectCount = 0;
        }
    }

    private void NextPart()
    {
        _altitude = 12 - _nextFallingCubes.Height;
        _xGrid = 0;
        _yGrid = 0;
        _fallingCubes = _nextFallingCubes;
        _fallingCubes.Cubes = _fallingCubes.Cubes.Select(cube => cube with { X = 0, Y = 0  }).ToList();
        if (CheckContact(_fallingCubes.Cubes, _altitude))
        {
            EndGame();
            return;
        }
        _nextFallingCubes = CreateNewFallingCube();
        var rand = new Random();
        var r = rand.Next(3);
        for (var i = 0; i < r; i++)
            Rotate(ref _nextFallingCubes);
        Cube.CreateCubes(NextPartOffset, 0, 5, ref _nextFallingCubes);
    }

    private void EndGame()
    {
        _gameTimer.Stop();
        End = true;
        _cubesInPlace.Clear();
        _nextFallingCubes.Clear();
        _fallingCubes.Clear();
        _cubesToDraw.Clear();
        Dispose();
        Shell.Current.GoToAsync($"..?End={End}");
    }

    private Part CreateNewFallingCube()
    {
        var rand = new Random();
        var r = rand.Next(100);

        int cubeId = _cubeProbabilities.FindLastIndex(i => i < r);
        var cubeData = CubeDictionnary[cubeId];
        CubeFallen[cubeId]++;

        return new(cubeData.Length, cubeData.Width, cubeData.Height, cubeData.Color);
    }
    private void UpdateFallingCube()
    {
        _fallingShadows = Cube.CreateCubes(_xGrid, _yGrid, _altitude, ref _fallingCubes);
        UpdateCubeToDraw();
    }
    private void UpdateCubeToDraw()
    {
        _cubesToDraw = _cubesInPlace.Concat(_fallingCubes.Cubes).Concat(_fallingShadows).Concat(_nextFallingCubes.Cubes).ToList();
        _cubesToDraw.Sort();
    }


    private void GameTimerTrigger(object sender, System.Timers.ElapsedEventArgs e)
        => UpdateObject();

    private void UpdateObject()
    {
        _altitude -= 1;
        
        Score += Level;
        TimePlayed += TimeSpan.FromMilliseconds( _gameTimer.Interval);
        if (CheckContact(_fallingCubes.Cubes, _altitude))
        {
            _cubesInPlace.AddRange(_fallingCubes.Cubes);
            if (CheckFullLine())
                return;
            NextPart();

        }
        var fib = 0;
        for (var i = 1; i <= Level; i++)
            fib += 1000 * i;
        if (Score  > fib - _startingfib)
        {
            Level++;
            _gameTimer.Interval = (10 - Level) * 100;
        }
        UpdateFallingCube();
    }

    private bool CheckFullLine()
    {
        for (var z = 0; z < DrawingHelper.GridSizeZ; z++)
        {
            var cubes = _cubesInPlace.Where(cube => cube.Z == z);
            if (cubes.Count() == DrawingHelper.GridSizeX * DrawingHelper.GridSizeY)
            {
                Score += 95 + Level *5;
                _cubesToDelete.Add(cubes);
            }
        }
        if (_cubesToDelete.Count > 0)
        {
            _gameTimer.Stop();
            _deleteLineTimer.Start();
            ClearedLayers++;
            return true;
        }
        return false;
    }

    private bool CheckContact(List<Cube> cubes, int z, int x = 0, int y = 0)
    {
        foreach (var cube in cubes)
            if (cube.X + x + 1 > DrawingHelper.GridSizeX
                    || cube.Y + y + 1 > DrawingHelper.GridSizeY
                    || z < 0
                    || _cubesInPlace.Where(cubeInPlace =>
                        cubeInPlace.X == cube.X + x
                        && cubeInPlace.Y == cube.Y + y
                        && cubeInPlace.Z == z
                        ).Any()
                )
                return true;
        return false;
    }
    [RelayCommand]
    internal void Rotate()
        => Rotate(ref _fallingCubes);
    internal void Rotate(ref Part part)
    {
        if (PauseText == "Resume")
            return;
        var width = part.Length;
        var length = part.Height;
        var height = part.Width;

        var cubesRotated = new Part(length, width, height, part.Color);
        var shadows = Cube.CreateCubes(_xGrid, _yGrid, _altitude, ref cubesRotated);
        if (CheckContact(cubesRotated.Cubes, _altitude))
        {
            for (var i = 1; i < cubesRotated.Length; i++)
            { 
                shadows = Cube.CreateCubes(_xGrid - i, _yGrid, _altitude, ref cubesRotated);
                if (!CheckContact(cubesRotated.Cubes, _altitude))
                {
                    _xGrid -= i; 
                    goto Found;
                }
            }
            for (var i = 1; i < cubesRotated.Width; i++)
            {
                shadows = Cube.CreateCubes(_xGrid, _yGrid - i, _altitude, ref cubesRotated);
                if (!CheckContact(cubesRotated.Cubes, _altitude))
                {
                    _yGrid -= i;
                    goto Found;
                }
            }
            cubesRotated.Height = width;
            cubesRotated.Width = length;
            cubesRotated.Length = height;
            shadows = Cube.CreateCubes(_xGrid, _yGrid, _altitude, ref cubesRotated);
            if (CheckContact(cubesRotated.Cubes, _altitude))
            {
                for (var i = 1; i < cubesRotated.Width; i++)
                {
                    shadows = Cube.CreateCubes(_xGrid, _yGrid - i, _altitude, ref cubesRotated);
                    if (!CheckContact(cubesRotated.Cubes, _altitude))
                    {
                        _yGrid -= i;
                        goto Found;
                    }
                }
                return;
            }
                
        }
        Found:
        part = cubesRotated;
        _fallingShadows = shadows;
        UpdateCubeToDraw();
    }
    [RelayCommand]
    internal void Translate(string direction)
    {
        if (PauseText == "Resume")
            return;
        int x = 0;
        int y = 0;
        switch (direction)
        {
            case "up":
                y = -1;
                break;
            case "down":
                y = 1;
                break;
            case "left":
                x = -1;
                break;
            case "right":
                x = 1;
                break;
        }
        if (((x > 0 && _xGrid + _fallingCubes.Length < GridSizeX) || x < 0 && _xGrid > 0) && !CheckContact(_fallingCubes.Cubes, _altitude, x, 0))
            _xGrid += x;
        if (((y > 0 && _yGrid + _fallingCubes.Width < GridSizeY) || y < 0 && _yGrid > 0) && !CheckContact(_fallingCubes.Cubes, _altitude, 0, y))
            _yGrid += y;
        UpdateFallingCube();
    }

    [RelayCommand]
    internal void Drop()
    {
        if (PauseText == "Resume")
            return;
        for ( int z = _altitude; z >= 0 ; z--)
        {

            if (CheckContact(_fallingCubes.Cubes, z: z - 1))
            {
                Score += Level * (_altitude - z) * 2;
                _altitude = z + 1;
                UpdateObject();
                return;
            }
        }
    }
    [RelayCommand]
    internal void Pause()
    {
        if (_gameTimer.Enabled)
        {
            PauseText = "Resume";
            _gameTimer.Stop(); 
        }
        else
        {
            PauseText = "Pause";
            _gameTimer.Start();
        }
    }

    private void OnKeyPressed(object sender, KeyboardHookEventArgs e)
    {
        var key = e.RawEvent.Keyboard.KeyCode;
        switch(key)
        {
            case SharpHook.Native.KeyCode.VcLeft:
                Translate("left");
                break;
            case SharpHook.Native.KeyCode.VcDown:
                Translate("down");
                break;
            case SharpHook.Native.KeyCode.VcRight :
                Translate("right");
                break;
            case SharpHook.Native.KeyCode.VcUp:
                Translate("up");
                break;
            case SharpHook.Native.KeyCode.VcD:
                Drop();
                break;
            case SharpHook.Native.KeyCode.VcS:
                Rotate();
                break;
        }
    }

    public void Dispose()
    {
        _hook.Dispose();
        _gameTimer.Dispose();
        _deleteLineTimer.Dispose();
    }
}
