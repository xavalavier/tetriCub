using Frac.ViewModel;
using Orbit.Engine;

namespace Frac.GameObjects;

public class DrawingSurface : GameObject
{
    private readonly GamePageViewModel _gamePageViewModel;
    private readonly List<DrawingPath> _drawingPaths= new List<DrawingPath>();

    internal void DrawGrid()
    {
        for (var i = 0; i < DrawingHelper.GridSizeX + 1; i++)
        {
            _drawingPaths.Add(DrawingHelper.GetIsoCoordinatesPath(i, 0, 0, i, DrawingHelper.GridSizeY, 0, thickness: 3));
            _drawingPaths.Add(DrawingHelper.GetIsoCoordinatesPath(i , 0, 0, i, 0, DrawingHelper.GridSizeZ, thickness: 3));
        }
        for (var i = 0; i < DrawingHelper.GridSizeY + 1; i++)
        {
            _drawingPaths.Add(DrawingHelper.GetIsoCoordinatesPath(0, i, 0, DrawingHelper.GridSizeX, i, 0,thickness:3));
            _drawingPaths.Add(DrawingHelper.GetIsoCoordinatesPath(0, i, 0, 0, i, DrawingHelper.GridSizeZ, thickness: 3));
        }
        for (var i = 0; i < DrawingHelper.GridSizeZ + 1; i++)
        {
            _drawingPaths.Add(DrawingHelper.GetIsoCoordinatesPath(0, 0, i, DrawingHelper.GridSizeX, 0, i, thickness: 3));
            _drawingPaths.Add(DrawingHelper.GetIsoCoordinatesPath(0, 0, i, 0, DrawingHelper.GridSizeY, i, thickness: 3));
        }

    }

    public DrawingSurface(GamePageViewModel GamePageViewModel)
    {
        _gamePageViewModel = GamePageViewModel;
        DrawGrid();
        DrawSpottingGrid();
    }

    private void DrawSpottingGrid()
    {
        _drawingPaths.Add(DrawingHelper.GetIsoCoordinatesPath(0, 0, DrawingHelper.GridSizeZ + 0.5f, DrawingHelper.GridSizeX, 0, DrawingHelper.GridSizeZ + 0.5f, thickness: 3));
        _drawingPaths.Add(DrawingHelper.GetIsoCoordinatesPath(0, 0, DrawingHelper.GridSizeZ + 0.5f, 0, DrawingHelper.GridSizeY, DrawingHelper.GridSizeZ + 0.5f, thickness: 3));
        _drawingPaths.Add(DrawingHelper.GetIsoCoordinatesPath(0, 0, DrawingHelper.GridSizeZ + 1, DrawingHelper.GridSizeX, 0, DrawingHelper.GridSizeZ + 1, thickness: 3));
        _drawingPaths.Add(DrawingHelper.GetIsoCoordinatesPath(0, 0, DrawingHelper.GridSizeZ + 1, 0, DrawingHelper.GridSizeY, DrawingHelper.GridSizeZ + 1, thickness: 3));
        for (var i = 0; i < DrawingHelper.GridSizeX + 1; i++)
            _drawingPaths.Add(DrawingHelper.GetIsoCoordinatesPath(i, 0, DrawingHelper.GridSizeZ + 0.5f, i, 0, DrawingHelper.GridSizeZ + 1, thickness: 3));
        for (var i = 0; i < DrawingHelper.GridSizeY + 1; i++)
            _drawingPaths.Add(DrawingHelper.GetIsoCoordinatesPath(0, i, DrawingHelper.GridSizeZ + 0.5f, 0, i, DrawingHelper.GridSizeZ + 1, thickness: 3));

    }

    public override void Render(ICanvas canvas, RectF dimensions)
    {
        base.Render(canvas, dimensions);
        if (_gamePageViewModel.End)
            Remove(this);

        foreach (var path in _drawingPaths)
        {
            DrawPath(path, canvas);
        }
        foreach (var cube in _gamePageViewModel.CubeToDraw)
            DrawRectangle(cube, canvas);

    }

    public override void Update(double millisecondsSinceLastUpdate) 
        => base.Update(millisecondsSinceLastUpdate);

    private void DrawPath(DrawingPath path, ICanvas canvas)
    {
        canvas.StrokeColor = DrawingHelper.SupportedColors[path.ColorIndex];
        canvas.StrokeSize = path.Thickness;
        canvas.StrokeLineCap = LineCap.Round;
        canvas.DrawPath(path.Path);
    }
    private void DrawRectangle(Cube cube, ICanvas canvas)
    {
        foreach (var path in cube.Paths)
        {
            canvas.StrokeColor = DrawingHelper.SupportedColors[path.ColorIndex];
            canvas.StrokeSize = 2;
            canvas.StrokeLineCap = LineCap.Square;
            canvas.FillColor = path.IsSide 
                ? cube.Color.WithLuminosity(0.3f) 
                : cube.Color;
            canvas.FillPath(path.Path);
            canvas.DrawPath(path.Path);
        }
    }
}
