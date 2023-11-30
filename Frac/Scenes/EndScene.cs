using Frac.GameObjects;
using Orbit.Engine;

namespace Frac.Scenes;

public class EndScene : GameScene
{
	public EndScene(
		EndGameDrawing endGameDrawing)
	{
		Add(endGameDrawing);
    }
}
