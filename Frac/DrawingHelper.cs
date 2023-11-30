
namespace Frac
{
    internal static class DrawingHelper
    {
#if WINDOWS
        private static readonly int _gridOffsetX = 1000;
        private static readonly int _gridOffsetY = 700;
        private static readonly int _tileSize = 40;
        public static int NextPartOffset { get; } = -10;
        public static int EndGameXOffset { get ;} =  - 23;
        public static int EndGameYOffset { get ;} =  13;
        public static int EndGameXFact { get ;} = 12;
        public static int EndGameYFact { get ;} = -8;
#else
        private static readonly int _gridOffsetX = 300;
        private static readonly int _gridOffsetY = 302;
        private static readonly int _tileSize = 19;
        public static int NextPartOffset { get; } = -7;
        public static int EndGameXOffset { get; } = -15;
        public static int EndGameYOffset { get; } = 13;
        public static int EndGameXFact { get; } = 9;
        public static int EndGameYFact { get; } = -7;
#endif

        private static readonly float _tileRatioX = 0.4f;
        private static readonly float _tileRatioY = 0.5f;

        public static int StartingLevel { get; set; } = 1;
        public static int StartingLayers { get; set; } = 0;
        public static int GridSizeX { get; } = 6;
        public static int GridSizeY { get; } = 6;
        public static int GridSizeZ { get; } = 12;

        internal static PointF GetPixelPointCoordinates(int X, int Y, float Z)
        => new
        (
            GetXCoordinates(X, Y),
            GetYCoordinates(Y, Z)
        );

        internal static DrawingPath GetIsoCoordinatesPath(int XGridStart, int YGridStart, float ZGridStart, int XGridEnd, int YGridEnd, float ZGridEnd, short colorIndex = 10,int thickness = 5, bool isSide = false)
            => new DrawingPath(
                colorIndex,
                thickness,
                GetPixelPointCoordinates(XGridStart, YGridStart, ZGridStart),
                GetPixelPointCoordinates(XGridEnd, YGridEnd, ZGridEnd),
                isSide
            );

        private static float GetXCoordinates(int XGrid, int YGrid)
        => _gridOffsetX + (XGrid - YGrid * _tileRatioX) * _tileSize;
        private static float GetYCoordinates(int YGrid, float ZGrid)
            => _gridOffsetY + YGrid * _tileSize * _tileRatioY - ZGrid * _tileSize;

        internal static List<Color> SupportedColors { get; } = new List<Color>
        {
            Colors.Black,
            Colors.Red,
            Colors.Orange,
            Colors.Yellow,
            Colors.DarkGreen,
            Colors.LightGreen,
            Colors.SlateBlue,
            Colors.Indigo,
            Colors.Violet,
            Colors.Cyan,
            Colors.White
        };

        public record CubesData(int Length, int Width, int Height, Color Color);
        public static Dictionary<int, CubesData> CubeDictionnary { get; } = new Dictionary<int, CubesData>()
        {
            [0] = new(1, 1, 1, SupportedColors[1]),
            [1] = new(1, 2, 1, SupportedColors[2]),
            [2] = new(1, 3, 1, SupportedColors[3]),
            [3] = new(2, 2, 1, SupportedColors[4]),
            [4] = new(2, 2, 2, SupportedColors[5]),
            [5] = new(1, 4, 1, SupportedColors[6]),
            [6] = new(3, 3, 1, SupportedColors[7]),
            [7] = new(3, 2, 2, SupportedColors[8]),
            [8] = new(3, 3, 3, SupportedColors[9]),
        };

    }
}
