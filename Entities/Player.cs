using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Entities;
using MonoGameLibrary.Input;
using MonoGameLibrary.Shapes;
using MonoGameLibrary.Misc;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Managers;
using MonoGameLibrary;
using System.Collections.Generic;

namespace DefaultGame.Entities;

public class Player : PlayerCharacter
{
    private float moveSpeed = 90;
    private float jumpSpeed = 300;

    private bool isGrounded = false;
    private List<Entity> floorEntities = new List<Entity>();

    private Trigger floorTrigger;

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
        useGravity = true;
        collisionType = CollisionType.DYNAMIC;

        collider = new Box(
            position.X,
            position.Y,
            sprite.Width,
            sprite.Height
        );

        floorTrigger = new Trigger(_entityName + " Floor Trigger");
        floorTrigger.LoadContent(Core.Content);
        floorTrigger.Initialize();
        floorTrigger.AttachTo(this);
        floorTrigger.Collider.Width = 6;
        floorTrigger.Collider.Height = 6;
        floorTrigger.DebugColor = Color.Red;
        floorTrigger.SetLayer(10);
        floorTrigger.SetRelativePosition(0.5f, 6.5f);
        floorTrigger.SetPosition(position);
        floorTrigger.Register();
        floorTrigger.onTriggerEnter += OnTriggerEnter;
        floorTrigger.onTriggerExit += OnTriggerExit;
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

        isGrounded = floorEntities.Count > 0;

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

        if (keyboard.WasKeyJustPressed(Keys.Space))
        {
            Jump();
        }

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

    private void Jump()
    {
        if (isGrounded)
        {
            Velocity.Y = -jumpSpeed;
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Entity other)
    {
        floorEntities.Add(other);
    }

    private void OnTriggerExit(Entity other)
    {
        floorEntities.Remove(other);
    }
}
