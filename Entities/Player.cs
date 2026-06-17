using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGameLibrary.Entities;
using MonoGameLibrary.Input;

namespace DefaultGame.Entities;

public class Player : PlayerCharacter
{
    public Player(string name) : base(name)
    {
    }

    public override void Initialize()
    {
        base.Initialize();

        _canUpdate = true;
    }

    public override void LoadContent(ContentManager content)
    {
        base.LoadContent(content);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        CheckKeyboardInput(deltaTime);
        CheckGamePadInput(deltaTime);
        CheckMouseInput(deltaTime);
    }

    private void CheckMouseInput(float deltaTime)
    {
        // Get a reference to the keyboard inof
        MouseInfo mouse = InputManager.Instance.Mouse;

    }

    private void CheckKeyboardInput(float deltaTime)
    {
        // Get a reference to the keyboard inof
        KeyboardInfo keyboard = InputManager.Instance.Keyboard;
    }

    private void CheckGamePadInput(float deltaTime)
    {
        // Get the gamepad info for gamepad one.
        GamePadInfo gamePadOne = InputManager.Instance.GamePads[(int)PlayerIndex.One];
    }
}
