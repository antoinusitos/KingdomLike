using DefaultGame.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using MonoGameLibrary;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.Managers;
using DefaultGame.Entities;
using DefaultGame.Misc;
using MonoGameLibrary.Graphics;

namespace DefaultGame.Scenes;

public class GameScene : Scene
{
    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        // During the game scene, we want to disable exit on escape. Instead,
        // the escape key will be used to return back to the title screen
        Core.ExitOnEscape = false;

        Rectangle screenBounds = Core.GraphicsDevice.PresentationParameters.Bounds;

        if (tilemap != null)
        {
            RoomBounds = new Rectangle(
                 (int)tilemap.TileWidth,
                 (int)tilemap.TileHeight,
                 screenBounds.Width - (int)tilemap.TileWidth * 2,
                 screenBounds.Height - (int)tilemap.TileHeight * 2
             );

            // Initial slime position will be the center tile of the tile map.
            int centerRow = tilemap.Rows / 2;
            int centerColumn = tilemap.Columns / 2;
        }

        DefaultGameGameManager.Instance.player = new Player("player");
        DefaultGameGameManager.Instance.player.LoadContent(Content);
        DefaultGameGameManager.Instance.player.Initialize();
        DefaultGameGameManager.Instance.player.Register();
        DefaultGameGameManager.Instance.player.SetPosition(100, 100);

        InitializeUI();

        CameraManager.Instance.Camera.Position = new Vector2(DefaultGameGameManager.TileSize * 5 * DefaultGameGameManager.GameScale, DefaultGameGameManager.TileSize * 5 * DefaultGameGameManager.GameScale);
    }

    public override void LoadContent()
    {
        // Create the tilemap from the XML configuration file.
        tilemap = RessourceManager.Instance.GetOrAddTilemap("images/tilemap-definition2.xml", out TilemapJSON tilemapJSON);
        if (tilemap != null)
        {
            tilemap.Scale = new Vector2(DefaultGameGameManager.GameScale, DefaultGameGameManager.GameScale);
        }
    }

    public override void Update(float deltaTime)
    {
        // Ensure the UI is always updated
        GumService.Default.Update(TimeManager.Instance.gameTime);

        // If the game is paused, do not continue
        if (DefaultGameGameManager.Instance.paused)
        {
            return;
        }

        // Check for keyboard input and handle it.
        CheckKeyboardInput();

        // Check for gamepad input and handle it.
        CheckGamePadInput();
    }

    private void CheckKeyboardInput()
    {
        // Get a reference to the keyboard inof
        KeyboardInfo keyboard = InputManager.Instance.Keyboard;

        // If the escape key is pressed, pause the game.
        if (keyboard.WasKeyJustPressed(Keys.Escape))
        {
            DefaultGameGameManager.Instance.PauseGame();
            return;
        }

        // If the escape key is pressed, return to the title screen.
        if (keyboard.WasKeyJustPressed(Keys.Escape))
        {
            SceneManager.Instance.ChangeScene(new TitleScene());
        }

        // If the M key is pressed, toggle mute state for audio.
        if (keyboard.WasKeyJustPressed(Keys.M))
        {
            Core.Audio.ToggleMute();
        }

        // If the + button is pressed, increase the volume.
        if (keyboard.WasKeyJustPressed(Keys.OemPlus))
        {
            Core.Audio.SongVolume += 0.1f;
            Core.Audio.SoundEffectVolume += 0.1f;
        }

        // If the - button was pressed, decrease the volume.
        if (keyboard.WasKeyJustPressed(Keys.OemMinus))
        {
            Core.Audio.SongVolume -= 0.1f;
            Core.Audio.SoundEffectVolume -= 0.1f;
        }
    }

    private void CheckGamePadInput()
    {
        // Get the gamepad info for gamepad one.
        GamePadInfo gamePadOne = InputManager.Instance.GamePads[(int)PlayerIndex.One];

        // If the start button is pressed, pause the game
        if (gamePadOne.WasButtonJustPressed(Buttons.Start))
        {
            DefaultGameGameManager.Instance.PauseGame();
            return;
        }
    }

    public override void Draw(float deltaTime)
    {
        // Draw the tilemap
        if (tilemap != null)
        {
            tilemap.Draw(Core.SpriteBatch);
        }
    }

    private void InitializeUI()
    {
        GumService.Default.Root.Children.Clear();

        UIManager.Instance.currentUIEntity = new GameSceneUI();

        UIManager.Instance.currentUIEntity.LoadContent(Content);

        ((GameSceneUI)UIManager.Instance.currentUIEntity).CreatePausePanel();
    }

}
