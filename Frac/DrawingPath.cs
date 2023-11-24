
namespace Frac;

public class DrawingPath
{
    public DrawingPath(short colorIndex, float thickness)
    {
        ColorIndex = colorIndex;
        Thickness = thickness;

        Path = new PathF();
    }
    public DrawingPath(short colorIndex, int thickness, PointF startPoint, PointF endPoint, bool isSide = false)
    {
        ColorIndex = colorIndex;
        Thickness = thickness;
        IsSide = isSide;

        Path = new PathF();
        Path.LineTo(startPoint);
        Path.LineTo(endPoint);
    }

    public short ColorIndex { get; }
    public PathF Path { get; }
    public float Thickness { get; }
    public bool IsSide { get; }

    public void Add(PointF point) => Path.LineTo(point);
}
