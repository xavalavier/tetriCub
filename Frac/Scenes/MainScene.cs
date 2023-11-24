using Frac.GameObjects;
using Orbit.Engine;

namespace Frac.Scenes;

public class MainScene : GameScene
{
	public MainScene(
		DrawingSurface drawingSurface)
	{
		Add(drawingSurface);
    }
}
