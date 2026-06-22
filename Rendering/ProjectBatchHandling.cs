using DefaultGame.Utilities;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;

namespace DefaultGame.Rendering;

public class ProjectBatchHandling : Singleton<ProjectBatchHandling>
{
    private SpriteBatch backgroundBatch;
    private SpriteBatch mainLayerBatch;
    private SpriteBatch foregroundBatch;
    private SpriteBatch lightBatch;
    private SpriteBatch occupancyBatch;
    private SpriteBatch screenBatch;

    public SpriteBatch BackgroundBatch => backgroundBatch;

    public SpriteBatch MainLayerBatch => mainLayerBatch;

    public SpriteBatch ForegroundBatch => foregroundBatch;

    public SpriteBatch LightBatch => lightBatch;

    public SpriteBatch OccupancyBatch => occupancyBatch;

    public SpriteBatch ScreenBatch => screenBatch;

    public ProjectBatchHandling()
    {
        backgroundBatch = new SpriteBatch(Core.GraphicsDevice);
        mainLayerBatch = new SpriteBatch(Core.GraphicsDevice);
        foregroundBatch = new SpriteBatch(Core.GraphicsDevice);
        lightBatch = new SpriteBatch(Core.GraphicsDevice);
        occupancyBatch = new SpriteBatch(Core.GraphicsDevice);
        screenBatch = new SpriteBatch(Core.GraphicsDevice);
    }
}