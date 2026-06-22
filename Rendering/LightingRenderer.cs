using System;
using System.Net.Mime;
using DefaultGame.Misc;
using DefaultGame.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Managers;

namespace DefaultGame.Rendering;

public class LightingRenderer : Singleton<LightingRenderer>
{
    private RenderTarget2D LightBuffer;
    private SpriteBatch LightBatch;
    private SpriteBatch ScreenBatch;
    private static Texture2D logo;

    public LightingRenderer()
    {
        Viewport viewport = Core.GraphicsDevice.Viewport;
        LightBuffer = new RenderTarget2D(
            graphicsDevice: Core.GraphicsDevice,
            width: viewport.Width,
            height: viewport.Height,
            mipMap: false,
            preferredFormat: SurfaceFormat.Color, 
            preferredDepthFormat: DepthFormat.None);
        LightBatch = new SpriteBatch(Core.GraphicsDevice);
        ScreenBatch = new SpriteBatch(Core.GraphicsDevice);
    }
    
    public static void LoadContent()
    {
        logo = Core.Content.Load<Texture2D>("images/logo");
    }

    public static void Render()
    {
        Core.GraphicsDevice.SetRenderTarget(instance.LightBuffer);  
        Core.GraphicsDevice.Clear(Color.AliceBlue);
        
        Instance.LightBatch.Begin();
        Instance.LightBatch.Draw(logo, Vector2.Zero, Color.White);
        Instance.LightBatch.End();
        Core.GraphicsDevice.SetRenderTarget(null);
    }

    public static void DebugRender()
    {
        Core.SpriteBatch.Begin();
        
        Core.SpriteBatch.Draw(Instance.LightBuffer, new Vector2(200, 50),
            new Rectangle(0, 0, 128, 128),
            Color.White);
        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
    }
}