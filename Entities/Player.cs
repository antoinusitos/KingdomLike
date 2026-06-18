using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Entities;
using MonoGameLibrary.Input;
using MonoGameLibrary.Shapes;
using MonoGameLibrary.Misc;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Managers;

namespace DefaultGame.Entities;

public class Player : PlayerCharacter
{
    private float moveSpeed = 75;

    public Player(string name) : base(name)
    {
    }

    public override void Initialize()
    {
        base.Initialize();

        canUpdate = true;
        canCollide = true;
        canRender = true;
        canMove = true;
        canInteract = true;
        collisionType = CollisionType.DYNAMIC;

        collider = new Box(
            position.X,
            position.Y,
            sprite.Width,
            sprite.Height
        );
    }

    public override void LoadContent(ContentManager content)
    {
        base.LoadContent(content);

        TextureAtlas atlas2 = RessourceManager.Instance.GetOrAddTextureAtlas("images/atlas-definition2.xml");

        sprite = RessourceManager.Instance.GetOrAddSprite("player", atlas2);
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

        if (keyboard.IsKeyDown(Keys.D))
        {
            SetPosition(position + Vector2.UnitX * deltaTime * moveSpeed);
        }
        else if (keyboard.IsKeyDown(Keys.A))
        {
            SetPosition(position - Vector2.UnitX * deltaTime * moveSpeed);
        }
    }

    private void CheckGamePadInput(float deltaTime)
    {
        // Get the gamepad info for gamepad one.
        GamePadInfo gamePadOne = InputManager.Instance.GamePads[(int)PlayerIndex.One];
    }
}
