using System;
using System.Collections.Generic;
using KingdomLike.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Managers;

namespace KingdomLike.Rendering;

public class ProjectBatchHandling : Singleton<ProjectBatchHandling>
{
    public enum BatchType
    {
        Screen,
        World,
    }
    
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

    private IEnumerable<(SpriteBatch, BatchType, RenderTarget2D)> Batches
    {
        get
        {
            yield return (BackgroundBatch, BatchType.World, null);
            yield return (MainLayerBatch, BatchType.World, null);
            yield return (ForegroundBatch, BatchType.World, null);
            yield return (LightBatch, BatchType.World, null);
        }
    }

    public ProjectBatchHandling()
    {
        backgroundBatch = new SpriteBatch(Core.GraphicsDevice);
        mainLayerBatch = new SpriteBatch(Core.GraphicsDevice);
        foregroundBatch = new SpriteBatch(Core.GraphicsDevice);
        lightBatch = new SpriteBatch(Core.GraphicsDevice);
        occupancyBatch = new SpriteBatch(Core.GraphicsDevice);
        screenBatch = new SpriteBatch(Core.GraphicsDevice);
    }

    public void BeginRendering()
    {
        foreach (var (spriteBatch, batchType, _) in Batches)
        {
            StartBatch(spriteBatch, batchType);
        }
    }

    private void StartBatch(SpriteBatch spriteBatch, BatchType batchType)
    {
        switch (batchType)
        {
            case BatchType.Screen:
                spriteBatch.Begin();
                break;
            case BatchType.World:
                spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: CameraManager.Instance.Camera.screenScaleMatrix, sortMode: SpriteSortMode.Deferred);
                break;
        }
    }

    public void EndRendering()
    {
        foreach (var (spriteBatch, _, target) in Batches)
        {
            Core.GraphicsDevice.SetRenderTarget(target);
            if (target != null)
            {
                Core.GraphicsDevice.Clear(Color.Black);
            }
            spriteBatch.End();
        }
        Core.GraphicsDevice.SetRenderTarget(null);  
    }
}