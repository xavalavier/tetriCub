

namespace Frac
{
    public record Cube(IList<DrawingPath> Paths, int X, int Y) : IComparable<Cube>
    {
        public Color Color { get; set; }
        public int Z { get; set; }
        public static Cube CreateCube(int XGridStart, int YGridStart, int ZGridStart, Color color)
        {
            var paths = new List<DrawingPath>();
            var path = DrawingHelper.GetIsoCoordinatesPath(XGridStart, YGridStart, ZGridStart + 1, XGridStart + 1, YGridStart, ZGridStart + 1, 0);
            path.Add(DrawingHelper.GetPixelPointCoordinates(XGridStart + 1, YGridStart + 1, ZGridStart + 1));
            path.Add(DrawingHelper.GetPixelPointCoordinates(XGridStart, YGridStart + 1, ZGridStart + 1));
            path.Add(DrawingHelper.GetPixelPointCoordinates(XGridStart, YGridStart, ZGridStart + 1));
            paths.Add(path);

            path = DrawingHelper.GetIsoCoordinatesPath(XGridStart + 1, YGridStart, ZGridStart + 1, XGridStart + 1, YGridStart, ZGridStart, 0, isSide: true);
            path.Add(DrawingHelper.GetPixelPointCoordinates(XGridStart + 1, YGridStart + 1, ZGridStart));
            path.Add(DrawingHelper.GetPixelPointCoordinates(XGridStart + 1, YGridStart + 1, ZGridStart + 1));
            path.Add(DrawingHelper.GetPixelPointCoordinates(XGridStart + 1, YGridStart, ZGridStart + 1));
            paths.Add(path);

            path = DrawingHelper.GetIsoCoordinatesPath(XGridStart, YGridStart + 1, ZGridStart + 1, XGridStart + 1, YGridStart + 1, ZGridStart + 1, 0, isSide: true);
            path.Add(DrawingHelper.GetPixelPointCoordinates(XGridStart + 1, YGridStart + 1, ZGridStart));
            path.Add(DrawingHelper.GetPixelPointCoordinates(XGridStart, YGridStart + 1, ZGridStart));
            path.Add(DrawingHelper.GetPixelPointCoordinates(XGridStart, YGridStart + 1, ZGridStart + 1));
            paths.Add(path);

            return new(paths, XGridStart, YGridStart) { Color = color , Z = ZGridStart};
        }
        public static List<Cube> CreateCubes(int XGridStart, int YGridStart, int ZGridStart, ref Part part, bool withShadows = true)
        {
            var cubes = new List<Cube>();

            var shadows = new List<Cube>();
            for (var k = 0; k < part.Height; k++)
                for (var i = 0; i < part.Width; i++)
                    for (var j = 0; j < part.Length; j++)
                        cubes.Add(CreateCube(XGridStart + j, YGridStart + i, ZGridStart + k, part.Color));
            part.Cubes = cubes;
            if (withShadows)
                shadows.AddRange(CreateShadows(XGridStart, YGridStart, part.Length, part.Width));
            return shadows;
        }

        private static List<Cube> CreateShadows(int XGridStart, int YGridStart, int length, int width)
        {
            var shadows = new List<Cube>();
            for (int i = 0; i < Math.Max(length, width); i++)
            {
                shadows.Add(CreateShadow(XGridStart + Math.Min(i, length -1), YGridStart + Math.Min(i, width-1)));
            }
            return shadows;
        }

        private static Cube CreateShadow(int x, int y)
        {
            var paths = new List<DrawingPath>();
            var path = DrawingHelper.GetIsoCoordinatesPath(x, 0, DrawingHelper.GridSizeZ + 0.5f, x + 1, 0, DrawingHelper.GridSizeZ + 0.5f, colorIndex: 10);
            path.Add(DrawingHelper.GetPixelPointCoordinates( x + 1, 0, DrawingHelper.GridSizeZ + 1));
            path.Add(DrawingHelper.GetPixelPointCoordinates(x, 0, DrawingHelper.GridSizeZ + 1));
            path.Add(DrawingHelper.GetPixelPointCoordinates(x, 0, DrawingHelper.GridSizeZ + 0.5f));
            paths.Add(path);

            path = DrawingHelper.GetIsoCoordinatesPath(0, y, DrawingHelper.GridSizeZ + 0.5f, 0, y + 1, DrawingHelper.GridSizeZ + 0.5f, colorIndex: 10);
            path.Add(DrawingHelper.GetPixelPointCoordinates(0, y + 1, DrawingHelper.GridSizeZ + 1));
            path.Add(DrawingHelper.GetPixelPointCoordinates(0, y, DrawingHelper.GridSizeZ + 1));
            path.Add(DrawingHelper.GetPixelPointCoordinates(0, y, DrawingHelper.GridSizeZ + 0.5f));
            paths.Add(path);

            return new(paths, x, y) { Color = Colors.White, Z = DrawingHelper.GridSizeZ + 1 };
        }

        public int CompareTo(Cube a)
        {
            var posA = a.X + (a.Y - 1) * DrawingHelper.GridSizeX + (a.Z - 1) * DrawingHelper.GridSizeY * DrawingHelper.GridSizeX;
            var posB = X + (Y - 1) * DrawingHelper.GridSizeX + (Z - 1) * DrawingHelper.GridSizeY * DrawingHelper.GridSizeX;
            return posB.CompareTo(posA);
        }
    }
    public class Part
    {
        public List<Cube> Cubes { get; set; } = new List<Cube>();
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Color Color { get; set; }

        public Part(int length, int width, int height, Color color)
        {
            Length = length;
            Width = width;
            Height = height;
            Color = color;
        }
        public void Clear() => Cubes.Clear();
    }
}
