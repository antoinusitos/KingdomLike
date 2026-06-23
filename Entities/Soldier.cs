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

namespace KingdomLike.Entities;

public class Soldier : Entity, IDamageable
{
    public ShootingStats shootingStats;

    private House destinationHouse = null;
    private Vector2 dest;
    private bool backToBase = false;

    Random r = new Random();
    public int random;

    private float life = 1;

    private float speed = 30;
    private float arrivedDist = 1;

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
}
