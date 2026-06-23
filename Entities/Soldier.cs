using Microsoft.Xna.Framework.Content;
using MonoGameLibrary.Entities;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Managers;
using KingdomLike.Misc;
using KingdomLike.Interfaces;
using KingdomLike.Rendering;
using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Utils;
using MonoGameLibrary.Misc;
using MonoGameLibrary;
using System.Transactions;

namespace KingdomLike.Entities;

public class Soldier : Entity, IDamageable
{
    public ShootingStats shootingStats;

    private House destinationHouse = null;
    private Vector2 dest;
    private bool backToBase = false;

    Random r = new Random();
    public int random;

    private Trigger trigger;

    private float life = 1;

    private float speed = 30;
    private float arrivedDist = 1;

    private bool mustShoot = false;
    private bool mustMove = true;

    public Soldier(string name) : base(name)
    {
        usedBatch = ProjectBatchHandling.Instance.MainLayerBatch;
    }

    public override void Initialize()
    {
        base.Initialize();

        canUpdate = true;
        canRender = true;
        canMove = true;
        canInteract = true;

        shootingStats.canShoot = true;
        shootingStats.fireRate = 0.2f;

        trigger = new Trigger(entityName + " Trigger");
        trigger.LoadContent(Core.Content);
        trigger.Initialize();
        trigger.AttachTo(this);
        trigger.SetRelativePosition(-trigger.Collider.Width + 52, -trigger.Collider.Height + 52);
        trigger.SetPosition(position);
        trigger.Register();

        SetLayer(2);

        KingdomLikeGameManager.instance.population++;
    }

    public override void LoadContent(ContentManager content)
    {
        base.LoadContent(content);

        TextureAtlas atlas2 = RessourceManager.Instance.GetOrAddTextureAtlas("images/atlas-definition2.xml");

        sprite = RessourceManager.Instance.GetOrAddSprite("soldier", atlas2);
    }

    public void TakeDamage(float damage, Entity fromEntity)
    {
        life -= damage;
        if (life <= 0)
        {
            UnRegister();
        }
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        CheckInteraction();

        CheckShoot(deltaTime);

        if (!mustMove)
        {
            return;
        }

        if (destinationHouse == null)
        {
            int random = r.Next(0, KingdomLikeGameManager.instance.houses.Count);

            destinationHouse = KingdomLikeGameManager.instance.houses[random];

            return;
        }

        if (!backToBase)
        {
            float x = position.X - destinationHouse.Position.X < 0 ? 1 : -1;
            SetPosition(position + Vector2.UnitX * speed * deltaTime * x);
            dest = destinationHouse.Position;
        }
        else
        {
            float x = position.X - KingdomLikeGameManager.instance.playerBase.Position.X < 0 ? 1 : -1;
            SetPosition(position + Vector2.UnitX * speed * deltaTime * x);
            dest = KingdomLikeGameManager.instance.playerBase.Position;
        }

        if (MathUtils.Dist(Position, dest) <= arrivedDist)
        {
            if (backToBase)
            {
                KingdomLikeGameManager.instance.gold += r.Next(1, 3);
                destinationHouse = null;
            }

            backToBase = !backToBase;
        }
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
        bullet.AddIgnoreCollision(trigger);
        bullet.SetSpeed(7);
        bullet.SetDirection(Vector2.UnitX);
    }

    private void CheckInteraction()
    {
        mustShoot = false;
        mustMove = true;

        for (int i = 0; i < trigger.entities.Count; i++)
        {
            if (trigger.entities[i].GetType() == typeof(Enemy))
            {
                mustShoot = true;
                mustMove = false;
                return;
            }
        }
    }
}
