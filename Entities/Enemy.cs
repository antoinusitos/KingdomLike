using Microsoft.Xna.Framework.Content;
using MonoGameLibrary.Entities;
using MonoGameLibrary.Shapes;
using MonoGameLibrary.Misc;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Managers;
using KingdomLike.Misc;
using KingdomLike.Interfaces;

namespace KingdomLike.Entities;

public class Enemy : Entity, IDamageable
{
    public ShootingStats shootingStats;
    private bool mustShoot = false;

    private float life = 1;

    public Enemy(string name) : base(name)
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

        shootingStats.canShoot = true;
        shootingStats.fireRate = 0.2f;

        SetLayer(2);
    }

    public override void LoadContent(ContentManager content)
    {
        base.LoadContent(content);

        TextureAtlas atlas2 = RessourceManager.Instance.GetOrAddTextureAtlas("images/atlas-definition2.xml");

        sprite = RessourceManager.Instance.GetOrAddSprite("enemy", atlas2);
    }

    public void TakeDamage(float damage, Entity fromEntity)
    {
        life -= damage;
        if (life <= 0)
        {
            UnRegister();
        }
    }
}
