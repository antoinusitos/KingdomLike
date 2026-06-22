using KingdomLike.Misc;
using KingdomLike.Scenes;
using Gum.DataTypes;
using Gum.Forms.Controls;
using Gum.Managers;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Managers;
using MonoGameLibrary.UI;
using System;

namespace KingdomLike.UI;

public class GameSceneUI : UIEntity
{
    private SpriteFont font;

    // A reference to the pause panel UI element so we can set its visibility
    // when the game is paused.
    private Panel pausePanel;

    // A reference to the resume button UI element so we can focus it
    // when the game is paused.
    private AnimatedButton resumeButton;

    // Reference to the texture atlas that we can pass to UI elements when they
    // are created.
    private TextureAtlas atlas;

    // The UI sound effect to play when a UI event is triggered.
    private SoundEffect uiSoundEffect;

    // The position to draw the dungeon text at.
    private Vector2 goldTextPos;

    // The origin to set for the dungeon text.
    private Vector2 goldTextOrigin;

    private Vector2 goldTextScale;
    private string textGold;

    private Vector2 populationTextScale;
    private string textPopulation;
    private Vector2 populationTextPos;

    public GameSceneUI()
    {
        Rectangle RoomBounds = SceneManager.Instance.ActiveScene.RoomBounds;
    }

    public override void LoadContent(ContentManager content)
    {
        base.LoadContent(content);

        // Create the texture atlas from the XML configuration file
        atlas = RessourceManager.Instance.GetOrAddTextureAtlas("images/atlas-definition.xml");

        // Load the font.
        font = RessourceManager.Instance.GetOrAddSpriteFont("fonts/04B_30");

        // Load the sound effect to play when ui actions occur.
        uiSoundEffect = RessourceManager.Instance.GetOrAddSoundEffect("audio/ui");

        goldTextPos = new Vector2(10, 10);

        populationTextPos = new Vector2(10, 40);

        // Create the texture atlas from the XML configuration file
        TextureAtlas _atlas2 = RessourceManager.Instance.GetOrAddTextureAtlas("images/atlas-definition2.xml");
    }

    public override void Render(SpriteBatch spriteBatch)
    {
        textGold = "Gold : " + KingdomLikeGameManager.instance.gold;

        textPopulation = "Population : " + KingdomLikeGameManager.instance.population;

        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw the title text on top of that at its original position
        Core.SpriteBatch.DrawString(font, textGold, goldTextPos, Color.White, 0.0f, goldTextOrigin, 1.0f, SpriteEffects.None, 1.0f);

        // Draw the title text on top of that at its original position
        Core.SpriteBatch.DrawString(font, textPopulation, populationTextPos, Color.White, 0.0f, goldTextOrigin, 1.0f, SpriteEffects.None, 1.0f);

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();

        GumService.Default.Draw();
    }

    public void PauseGame()
    {
        // Make the pause panel UI element visible.
        pausePanel.IsVisible = true;

        // Set the resume button to have focus
        resumeButton.IsFocused = true;
    }

    public void CreatePausePanel()
    {
        pausePanel = new Panel();
        pausePanel.Anchor(Anchor.Center);
        pausePanel.WidthUnits = DimensionUnitType.Absolute;
        pausePanel.HeightUnits = DimensionUnitType.Absolute;
        pausePanel.Height = 70;
        pausePanel.Width = 264;
        pausePanel.IsVisible = false;
        pausePanel.AddToRoot();

        TextureRegion backgroundRegion = atlas.GetRegion("panel-background");

        NineSliceRuntime background = new NineSliceRuntime();
        background.Dock(Dock.Fill);
        background.Texture = backgroundRegion.Texture;
        background.TextureAddress = TextureAddress.Custom;
        background.TextureHeight = backgroundRegion.Height;
        background.TextureLeft = backgroundRegion.SourceRectangle.Left;
        background.TextureTop = backgroundRegion.SourceRectangle.Top;
        background.TextureWidth = backgroundRegion.Width;
        pausePanel.AddChild(background);

        TextRuntime textInstance = new TextRuntime();
        textInstance.Text = "PAUSED";
        textInstance.CustomFontFile = @"fonts/04b_30.fnt";
        textInstance.UseCustomFont = true;
        textInstance.FontScale = 0.5f;
        textInstance.X = 10f;
        textInstance.Y = 10f;
        pausePanel.AddChild(textInstance);

        resumeButton = new AnimatedButton(atlas);
        resumeButton.Text = "RESUME";
        resumeButton.Anchor(Anchor.BottomLeft);
        resumeButton.X = 9f;
        resumeButton.Y = -9f;
        resumeButton.Width = 80;
        resumeButton.Click += HandleResumeButtonClicked;
        pausePanel.AddChild(resumeButton);

        AnimatedButton quitButton = new AnimatedButton(atlas);
        quitButton.Text = "QUIT";
        quitButton.Anchor(Anchor.BottomRight);
        quitButton.X = -9f;
        quitButton.Y = -9f;
        quitButton.Width = 80;
        quitButton.Click += HandleQuitButtonClicked;

        pausePanel.AddChild(quitButton);
    }

    private void HandleResumeButtonClicked(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(uiSoundEffect);

        // Make the pause panel invisible to resume the game.
        pausePanel.IsVisible = false;
    }

    private void HandleQuitButtonClicked(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(uiSoundEffect);

        // Go back to the title scene.
        SceneManager.Instance.ChangeScene(new TitleScene());
    }
}
