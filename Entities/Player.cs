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
using MonoGameLibrary.Utils;
using MonoGameLibrary.Interfaces;
using System;
using DefaultGame.Rendering;
using KingdomLike.Misc;

namespace KingdomLike.Entities;

public class Player : PlayerCharacter
{
    private float moveSpeed = 90;
    private const float acceleration = 1000;
    private const float friction = 400;
    private float jumpSpeed = 300;

    private bool isGrounded = false;
    private List<Entity> floorEntities = new List<Entity>();

    private Trigger floorTrigger;

    private Trigger trigger;

    private InteractionPlayerFeedback interaction = null;

    private const string InteractionType = "IInteractable";

    public ShootingStats shootingStats;
    private bool mustShoot = false;

    public Player(string name) : base(name)
    {
        UsedBatch = ProjectBatchHandling.Instance.MainLayerBatch;
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

        shootingStats.canShoot = true;
        shootingStats.fireRate = 0.2f;

        SetLayer(5);

        floorTrigger = new Trigger(entityName + " Floor Trigger");
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

        trigger = new Trigger(entityName + " Trigger");
        trigger.LoadContent(Core.Content);
        trigger.Initialize();
        trigger.AttachTo(this);
        trigger.SetRelativePosition(-trigger.Collider.Width + 52, -trigger.Collider.Height + 52);
        trigger.SetPosition(position);
        trigger.Register();

        interaction = new InteractionPlayerFeedback("Interaction");
        interaction.LoadContent(Core.Content);
        interaction.Initialize();
        interaction.AttachTo(this);
        interaction.Register();
        interaction.SetRelativePosition(0, -20);
        interaction.SetPosition(position);
        interaction.SetScale(0.25f);
        interaction.SetActive(false);

        floorTrigger.IgnoreCollisions.Add(interaction);
        floorTrigger.IgnoreCollisions.Add(trigger);
        floorTrigger.IgnoreCollisions.Add(this);

        interaction.IgnoreCollisions.Add(floorTrigger);
        interaction.IgnoreCollisions.Add(trigger);
        interaction.IgnoreCollisions.Add(this);

        trigger.IgnoreCollisions.Add(floorTrigger);
        trigger.IgnoreCollisions.Add(interaction);
        trigger.IgnoreCollisions.Add(this);
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

        Velocity.X = Approach(Velocity.X, 0, friction * deltaTime);

        if (Velocity != Vector2.Zero)
        {
            SceneManager.Instance.SetIsDirty(true);
        }

        CheckInteraction();

        CheckShoot(deltaTime);
    }

    private void CheckShoot(float deltaTime)
    {
        if (!shootingStats.canShoot)
        {
            shootingStats.currentWait += deltaTime;
            if (shootingStats.currentWait >= shootingStats.fireRate)
            {
                shootingStats.currentWait = 0;
                shootingStats.canShoot = true;
            }
            return;
        }

        if (!mustShoot)
        {
            return;
        }

        shootingStats.canShoot = false;

        Bullet bullet = new Bullet("Bullet");
        bullet.LoadContent(Core.Content);
        bullet.Initialize();
        bullet.SetPosition(position);
        bullet.Register();
        bullet.SetOwner(this);
        bullet.SetSpeed(7);
        bullet.SetDirection(Vector2.UnitX);
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

        float inputX = 0;

        // If the A or Left keys are down, move the slime left on the screen.
        if (keyboard.IsKeyDown(Keys.A))
        {
            inputX = -1;
        }

        // If the D or Right keys are down, move the slime right on the screen.
        if (keyboard.IsKeyDown(Keys.D))
        {
            inputX = 1;
        }

        if (inputX != 0)
        {
            Velocity.X = Approach(Velocity.X, inputX * moveSpeed, acceleration * deltaTime);
        }

        if (keyboard.WasKeyJustPressed(Keys.F))
        {
            SetWantToInteract(true);
        }
    }

    private void CheckGamePadInput(float deltaTime)
    {
        // Get the gamepad info for gamepad one.
        GamePadInfo gamePadOne = InputManager.Instance.GamePads[(int)PlayerIndex.One];
    }

    private float Approach(float value, float target, float amount)
    {
        if (value < target)
            return Math.Min(value + amount, target);
        else
            return Math.Max(value - amount, target);
    }

    private void CheckInteraction()
    {
        mustShoot = false;
        interaction.SetActive(false);
        for (int i = 0; i < trigger.entities.Count; i++)
        {
            if (trigger.entities[i].GetType().GetInterface(InteractionType) != null)
            {
                interaction.SetActive(true);
            }
            if (trigger.entities[i].GetType() == typeof(Enemy))
            {
                mustShoot = true;
            }
        }
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

    public override void InteractWithTarget()
    {
        if (trigger.entities.Count <= 0)
        {
            return;
        }

        float dist = 9999999;

        for (int i = 0; i < trigger.entities.Count; i++)
        {
            if (trigger.entities[i].GetType().GetInterface(InteractionType) == null)
                continue;

            float tempDist = MathUtils.Dist(trigger.entities[i].Position, Position);
            if (dist > tempDist)
            {
                dist = tempDist;
                interactionTarget = (IInteractable)trigger.entities[i];
            }
        }

        if (interactionTarget != null)
        {
            interactionTarget.OnInteract(this);
        }
    }
}
