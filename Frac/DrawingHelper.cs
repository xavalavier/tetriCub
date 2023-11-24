
namespace Frac
{
    internal static class DrawingHelper
    {
#if WINDOWS
        private static readonly int _gridOffsetX = 1000;
        private static readonly int _gridOffsetY = 700;
        private static readonly int _tileSize = 40;
#else
        private static readonly int _gridOffsetX = 300;
        private static readonly int _gridOffsetY = 280;
        private static readonly int _tileSize = 21;
#endif

        private static readonly float _tileRatioX = 0.4f;
        private static readonly float _tileRatioY = 0.5f;

        public static int Level { get; set; } = 1;
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
    }
}
