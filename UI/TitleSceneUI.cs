using DefaultGame.Scenes;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Managers;
using MonoGameLibrary.UI;
using System;

namespace DefaultGame.UI;
public class TitleSceneUI : UIEntity
{
    private const string TITLE_TEXT = "Kingdom Like";
    private const string PRESS_ENTER_TEXT = "Press Enter To Start";

    // The font to use to render normal text.
    private SpriteFont font;

    // The font used to render the title text.
    private SpriteFont font5x;

    // The position to draw the dungeon text at.
    private Vector2 titleTextPos;

    // The origin to set for the dungeon text.
    private Vector2 titleTextOrigin;

    // The position to draw the press enter text at.
    private Vector2 pressEnterPos;

    // The origin to set for the press enter text when drawing it.
    private Vector2 pressEnterOrigin;

    // The texture used for the background pattern.
    private Texture2D backgroundPattern;

    // The destination rectangle for the background pattern to fill.
    private Rectangle backgroundDestination;

    // The offset to apply when drawing the background pattern so it appears to
    // be scrolling.
    private Vector2 backgroundOffset;

    // The speed that the background pattern scrolls.
    private float scrollSpeed = 50.0f;

    private SoundEffect uiSoundEffect;
    private Panel titleScreenButtonsPanel;
    private Panel optionsPanel;

    // The options button used to open the options menu.
    private AnimatedButton optionsButton;

    // The back button used to exit the options menu back to the title menu.
    private AnimatedButton optionsBackButton;

    // Reference to the texture atlas that we can pass to UI elements when they
    // are created.
    private TextureAtlas atlas;

    // The background theme song.
    private Song themeSong;

    public override void LoadContent(ContentManager content)
    {
        base.LoadContent(content);

        // Load the font for the standard text.
        font = RessourceManager.Instance.GetOrAddSpriteFont("fonts/04B_30");

        // Load the font for the title text.
        font5x = RessourceManager.Instance.GetOrAddSpriteFont("fonts/04B_30_5x");

        // Load the background pattern texture.
        backgroundPattern = RessourceManager.Instance.GetOrAddTexture2D("images/background-pattern");

        // Load the sound effect to play when ui actions occur.
        uiSoundEffect = RessourceManager.Instance.GetOrAddSoundEffect("audio/ui");

        // Load the texture atlas from the xml configuration file.
        atlas = RessourceManager.Instance.GetOrAddTextureAtlas("images/atlas-definition.xml");

        // Load the background theme music.
        themeSong = RessourceManager.Instance.GetOrAddSong("audio/theme");

        // While on the title screen, we can enable exit on escape so the player
        // can close the game by pressing the escape key.
        Core.ExitOnEscape = true;

        // Set the position and origin for the Dungeon text.
        Vector2 size = font5x.MeasureString(TITLE_TEXT);
        titleTextPos = new Vector2(640, 100);
        titleTextOrigin = size * 0.5f;

        // Set the position and origin for the press enter text.
        size = font.MeasureString(PRESS_ENTER_TEXT);
        pressEnterPos = new Vector2(640, 420);
        pressEnterOrigin = size * 0.5f;

        // Initialize the offset of the background pattern at zero.
        backgroundOffset = Vector2.Zero;

        // Set the background pattern destination rectangle to fill the entire
        // screen background.
        backgroundDestination = Core.GraphicsDevice.PresentationParameters.Bounds;

        // Start playing the background music.
        //Core.Audio.PlaySong(_themeSong);
    }

    public override void Render(SpriteBatch spriteBatch)
    {
        Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        // Draw the background pattern first using the PointWrap sampler state.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointWrap);
        Core.SpriteBatch.Draw(backgroundPattern, backgroundDestination, new Rectangle(backgroundOffset.ToPoint(), backgroundDestination.Size), Color.White * 0.5f);
        Core.SpriteBatch.End();

        if (titleScreenButtonsPanel.IsVisible)
        {
            // Begin the sprite batch to prepare for rendering.
            Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // The color to use for the drop shadow text.
            Color dropShadowColor = Color.Black * 0.5f;

            // Draw the title text slightly offset from it is original position and
            // with a transparent color to give it a drop shadow
            Core.SpriteBatch.DrawString(font5x, TITLE_TEXT, titleTextPos + new Vector2(10, 10), dropShadowColor, 0.0f, titleTextOrigin, 1.0f, SpriteEffects.None, 1.0f);

            // Draw the title text on top of that at its original position
            Core.SpriteBatch.DrawString(font5x, TITLE_TEXT, titleTextPos, Color.White, 0.0f, titleTextOrigin, 1.0f, SpriteEffects.None, 1.0f);

            // Draw the Slime text slightly offset from it is original position and
            // with a transparent color to give it a drop shadow
            Core.SpriteBatch.DrawString(font, PRESS_ENTER_TEXT, pressEnterPos + new Vector2(10, 10), dropShadowColor, 0.0f, pressEnterOrigin, 1.0f, SpriteEffects.None, 1.0f);

            // Draw the Slime text on top of that at its original position
            Core.SpriteBatch.DrawString(font, PRESS_ENTER_TEXT, pressEnterPos, Color.White, 0.0f, pressEnterOrigin, 1.0f, SpriteEffects.None, 1.0f);

            // Always end the sprite batch when finished.
            Core.SpriteBatch.End();
        }

        GumService.Default.Draw();
    }

    public void CreateTitlePanel()
    {
        // Create a container to hold all of our buttons
        titleScreenButtonsPanel = new Panel();
        titleScreenButtonsPanel.Dock(Gum.Wireframe.Dock.Fill);
        titleScreenButtonsPanel.AddToRoot();

        AnimatedButton startButton = new AnimatedButton(atlas);
        startButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        startButton.X = 50;
        startButton.Y = -12;
        startButton.Width = 70;
        startButton.Text = "Start";
        startButton.Click += HandleStartClicked;
        titleScreenButtonsPanel.AddChild(startButton);

        optionsButton = new AnimatedButton(atlas);
        optionsButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        optionsButton.X = -50;
        optionsButton.Y = -12;
        optionsButton.Width = 70;
        optionsButton.Text = "Options";
        optionsButton.Click += HandleOptionsClicked;
        titleScreenButtonsPanel.AddChild(optionsButton);

        startButton.IsFocused = true;
    }

    private void HandleStartClicked(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(uiSoundEffect);

        // Change to the game scene to start the game.
        SceneManager.Instance.ChangeScene(new GameScene());
    }

    private void HandleOptionsClicked(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(uiSoundEffect);

        // Set the title panel to be invisible.
        titleScreenButtonsPanel.IsVisible = false;

        // Set the options panel to be visible.
        optionsPanel.IsVisible = true;

        // Give the back button on the options panel focus.
        optionsBackButton.IsFocused = true;
    }

    public void CreateOptionsPanel()
    {
        optionsPanel = new Panel();
        optionsPanel.Dock(Gum.Wireframe.Dock.Fill);
        optionsPanel.IsVisible = false;
        optionsPanel.AddToRoot();

        TextRuntime optionsText = new TextRuntime();
        optionsText.X = 10;
        optionsText.Y = 10;
        optionsText.Text = "OPTIONS";
        optionsText.UseCustomFont = true;
        optionsText.FontScale = 0.5f;
        optionsText.CustomFontFile = @"fonts/04b_30.fnt";
        optionsPanel.AddChild(optionsText);

        OptionsSlider musicSlider = new OptionsSlider(atlas);
        musicSlider.Name = "MusicSlider";
        musicSlider.Text = "MUSIC";
        musicSlider.Anchor(Gum.Wireframe.Anchor.Top);
        musicSlider.Y = 30f;
        musicSlider.Minimum = 0;
        musicSlider.Maximum = 1;
        musicSlider.Value = Core.Audio.SongVolume;
        musicSlider.SmallChange = .1;
        musicSlider.LargeChange = .2;
        musicSlider.ValueChanged += HandleMusicSliderValueChanged;
        musicSlider.ValueChangeCompleted += HandleMusicSliderValueChangeCompleted;
        optionsPanel.AddChild(musicSlider);

        OptionsSlider sfxSlider = new OptionsSlider(atlas);
        sfxSlider.Name = "SfxSlider";
        sfxSlider.Text = "SFX";
        sfxSlider.Anchor(Gum.Wireframe.Anchor.Top);
        sfxSlider.Y = 93;
        sfxSlider.Minimum = 0;
        sfxSlider.Maximum = 1;
        sfxSlider.Value = Core.Audio.SoundEffectVolume;
        sfxSlider.SmallChange = .1;
        sfxSlider.LargeChange = .2;
        sfxSlider.ValueChanged += HandleSfxSliderChanged;
        sfxSlider.ValueChangeCompleted += HandleSfxSliderChangeCompleted;
        optionsPanel.AddChild(sfxSlider);

        optionsBackButton = new AnimatedButton(atlas);
        optionsBackButton.Text = "BACK";
        optionsBackButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        optionsBackButton.X = -28f;
        optionsBackButton.Y = -10f;
        optionsBackButton.Click += HandleOptionsButtonBack;
        optionsPanel.AddChild(optionsBackButton);
    }

    private void HandleSfxSliderChanged(object sender, EventArgs args)
    {
        // Intentionally not playing the UI sound effect here so that it is not
        // constantly triggered as the user adjusts the slider's thumb on the
        // track.

        // Get a reference to the sender as a Slider.
        var slider = (Slider)sender;

        // Set the global sound effect volume to the value of the slider.;
        Core.Audio.SoundEffectVolume = (float)slider.Value;
    }

    private void HandleSfxSliderChangeCompleted(object sender, EventArgs e)
    {
        // Play the UI Sound effect so the player can hear the difference in audio.
        Core.Audio.PlaySoundEffect(uiSoundEffect);
    }

    private void HandleMusicSliderValueChanged(object sender, EventArgs args)
    {
        // Intentionally not playing the UI sound effect here so that it is not
        // constantly triggered as the user adjusts the slider's thumb on the
        // track.

        // Get a reference to the sender as a Slider.
        var slider = (Slider)sender;

        // Set the global song volume to the value of the slider.
        Core.Audio.SongVolume = (float)slider.Value;
    }

    private void HandleMusicSliderValueChangeCompleted(object sender, EventArgs args)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(uiSoundEffect);
    }

    private void HandleOptionsButtonBack(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(uiSoundEffect);

        // Set the title panel to be visible.
        titleScreenButtonsPanel.IsVisible = true;

        // Set the options panel to be invisible.
        optionsPanel.IsVisible = false;

        // Give the options button on the title panel focus since we are coming
        // back from the options screen.
        optionsButton.IsFocused = true;
    }

    public override void Update(float deltaTime)
    {

        // Update the offsets for the background pattern wrapping so that it
        // scrolls down and to the right.
        float offset = scrollSpeed * deltaTime;
        backgroundOffset.X -= offset;
        backgroundOffset.Y -= offset;

        // Ensure that the offsets do not go beyond the texture bounds so it is
        // a seamless wrap.
        backgroundOffset.X %= backgroundPattern.Width;
        backgroundOffset.Y %= backgroundPattern.Height;

        GumService.Default.Update(TimeManager.Instance.gameTime);
    }

}
