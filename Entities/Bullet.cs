using KingdomLike.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGameLibrary;
using MonoGameLibrary.Entities;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Managers;
using MonoGameLibrary.Misc;

namespace KingdomLike.Entities;

public class Bullet : Entity
{
    private Trigger trigger;

    private Vector2 direction;
    private float moveSpeed;

    private float duration = 3;
    private float currentDuration = 0;

    private Entity owner;

    private const string IDamageableName = "IDamageable";

    public Bullet(string name) : base(name)
    {
    }

    public override void Initialize()
    {
        base.Initialize();

        canRender = true;
        canMove = true;
        canUpdate = true;

        trigger = new Trigger(entityName + " Trigger");
        trigger.LoadContent(Core.Content);
        trigger.Initialize();
        trigger.SetTriggerSize(new Vector2(sprite.Width, sprite.Height));
        trigger.AttachTo(this);
        trigger.SetRelativePosition(0, 0);
        trigger.SetPosition(position);
        trigger.Register();
        trigger.SetLayer(10);
        trigger.onTriggerEnter += OnTriggerEnter;
        trigger.IgnoreCollisions.Add(this);
    }

    public override void LoadContent(ContentManager content)
    {
        base.LoadContent(content);

        // Create the texture atlas from the XML configuration file
        TextureAtlas atlas2 = RessourceManager.Instance.GetOrAddTextureAtlas("images/atlas-definition2.xml");

        sprite = RessourceManager.Instance.GetOrAddSprite("health", atlas2);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        currentDuration += deltaTime;
        if (currentDuration >= duration)
        {
            UnRegister();
        }
    }

    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
        Velocity = direction * moveSpeed;
    }

    public void SetDirection(Vector2 newDir)
    {
        direction = newDir;
        Velocity = direction * moveSpeed;
    }

    private void OnTriggerEnter(Entity other)
    {
        if (other.GetType().GetInterface(IDamageableName) == null)
            return;

        IDamageable damageable = (IDamageable)other;
        damageable.TakeDamage(0.33f, owner);

        UnRegister();
    }

    public void SetOwner(Entity other)
    {
        owner = other;
        trigger.IgnoreCollisions.Add(owner);
    }
}
