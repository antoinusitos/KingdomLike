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

    #region Sprite Batch

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
            yield return (BackgroundBatch, BatchType.World, backgroundBuffer);
            yield return (MainLayerBatch, BatchType.World, mainLayerBuffer);
            yield return (ForegroundBatch, BatchType.World, foregroundBuffer);
            yield return (LightBatch, BatchType.World, lightBuffer);
        }
    }

    #endregion

    #region Render Targets

    private RenderTarget2D lightBuffer;
    private RenderTarget2D backgroundBuffer;
    private RenderTarget2D mainLayerBuffer;
    private RenderTarget2D foregroundBuffer;

    public RenderTarget2D LightBuffer => lightBuffer;

    public RenderTarget2D BackgroundBuffer => backgroundBuffer;

    public RenderTarget2D MainLayerBuffer => mainLayerBuffer;

    public RenderTarget2D ForegroundBuffer => foregroundBuffer;
    public int FrameCountInclusive => 3;

    public Texture2D GetBuffer(int frameIndex) => frameIndex switch
    {
        0 => BackgroundBuffer,
        1 => MainLayerBuffer,
        2 => ForegroundBuffer,
        3 => LightBuffer
    };

    #endregion

    public ProjectBatchHandling()
    {
        InitializeSpriteBatch();
        InitializeRenderTarget();
    }

    private void InitializeSpriteBatch()
    {
        backgroundBatch = new SpriteBatch(Core.GraphicsDevice);
        mainLayerBatch = new SpriteBatch(Core.GraphicsDevice);
        foregroundBatch = new SpriteBatch(Core.GraphicsDevice);
        lightBatch = new SpriteBatch(Core.GraphicsDevice);
        occupancyBatch = new SpriteBatch(Core.GraphicsDevice);
        screenBatch = new SpriteBatch(Core.GraphicsDevice);
    }
    
    private void InitializeRenderTarget()
    {
        InitializeBuffer(ref lightBuffer);
        InitializeBuffer(ref backgroundBuffer);
        InitializeBuffer(ref mainLayerBuffer);
        InitializeBuffer(ref foregroundBuffer);
    }

    private void InitializeBuffer(ref RenderTarget2D output)
    {
        Viewport viewport = Core.GraphicsDevice.Viewport;
        output = new RenderTarget2D(
            graphicsDevice: Core.GraphicsDevice,
            width: viewport.Width,
            height: viewport.Height,
            mipMap: false,
            preferredFormat: SurfaceFormat.Color, 
            preferredDepthFormat: DepthFormat.None);
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
                spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: CameraManager.Instance.Camera.GetTransformation(Core.GraphicsDevice), sortMode: SpriteSortMode.Deferred);
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
                Core.GraphicsDevice.Clear(Color.Transparent);
            }
            spriteBatch.End();
        }
        Core.GraphicsDevice.SetRenderTarget(null);  
    }
}