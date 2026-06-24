using System;
using System.Collections.Generic;
using System.Linq;
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

    public struct BatcherData
    {
        public SpriteBatch Batcher;
        public BatchType BatchType;
        public Effect Effect;
        public RenderTarget2D Buffer;
        public string Name;

        public static implicit operator BatcherData((SpriteBatch, BatchType, Effect, RenderTarget2D, string) tuple) => new() {
                Batcher = tuple.Item1,
                BatchType = tuple.Item2,
                Effect = tuple.Item3,
                Buffer = tuple.Item4,
                Name = tuple.Item5
            };
    }

    public static Effect AlphaDrawerEffect { get; private set; }
    
    
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

    private IEnumerable<BatcherData> Batches
    {
        get
        {
            yield return (BackgroundBatch, BatchType.World, null, backgroundBuffer, nameof(BackgroundBatch));
            yield return (MainLayerBatch, BatchType.World, null, mainLayerBuffer, nameof(MainLayerBatch));
            yield return (ForegroundBatch, BatchType.World, null, foregroundBuffer, nameof(ForegroundBatch));
            yield return (LightBatch, BatchType.World, null, lightBuffer, nameof(LightBatch));
            yield return (OccupancyBatch, BatchType.World, AlphaDrawerEffect, occupancyBuffer, nameof(OccupancyBatch));
        }
    }

    #endregion

    #region Render Targets

    private RenderTarget2D lightBuffer;
    private RenderTarget2D backgroundBuffer;
    private RenderTarget2D mainLayerBuffer;
    private RenderTarget2D foregroundBuffer;
    private RenderTarget2D occupancyBuffer;

    public RenderTarget2D LightBuffer => lightBuffer;

    public RenderTarget2D BackgroundBuffer => backgroundBuffer;

    public RenderTarget2D MainLayerBuffer => mainLayerBuffer;

    public RenderTarget2D ForegroundBuffer => foregroundBuffer;
    public int FrameCountInclusive => Batches.Count() - 1;

    public BatcherData GetBatcherData(int frameIndex) => Batches.ElementAt(frameIndex);
    
    public Texture2D GetBuffer(int frameIndex) => Batches.ElementAt(frameIndex).Buffer;

    #endregion

    public ProjectBatchHandling()
    {

    }
    
    public void LoadContent()
    {
        AlphaDrawerEffect = Core.Content.Load<Effect>("effects/AlphaDrawer");
    }

    public void InitializeResources()
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
        InitializeBuffer(ref occupancyBuffer);
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
        foreach (BatcherData data in Batches)
        {
            StartBatch(data);
        }
    }

    private void StartBatch(BatcherData data)
    {
        switch (data.BatchType)
        {
            case BatchType.Screen:
                data.Batcher.Begin(effect: data.Effect);
                break;
            case BatchType.World:
                data.Batcher.Begin(
                    samplerState: SamplerState.PointClamp, 
                    transformMatrix: CameraManager.Instance.Camera.GetTransformation(Core.GraphicsDevice), 
                    sortMode: SpriteSortMode.Deferred,
                    effect: data.Effect
                    );
                break;
        }
    }

    public void EndRendering()
    {
        foreach (BatcherData data in Batches)
        {
            Core.GraphicsDevice.SetRenderTarget(data.Buffer);
            if (data.Buffer != null)
            {
                Core.GraphicsDevice.Clear(Color.Transparent);
            }
            data.Batcher.End();
        }
        Core.GraphicsDevice.SetRenderTarget(null);  
    }
}