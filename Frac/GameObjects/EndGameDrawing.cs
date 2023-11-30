using Frac.ViewModel;
using Orbit.Engine;

namespace Frac.GameObjects;

public class EndGameDrawing : GameObject
{
    private readonly EndGamePageViewModel _endGamePageViewModel;

    public EndGameDrawing(EndGamePageViewModel EndGamePageViewModel)
    {
        _endGamePageViewModel = EndGamePageViewModel;
    }

    public override void Render(ICanvas canvas, RectF dimensions)
    {
        base.Render(canvas, dimensions);

        foreach (var cube in _endGamePageViewModel.CubeToDraw)
            DrawRectangle(cube, canvas);
    }

    public override void Update(double millisecondsSinceLastUpdate) 
        => base.Update(millisecondsSinceLastUpdate);

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
