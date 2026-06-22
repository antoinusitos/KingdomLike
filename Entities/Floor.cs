using DefaultGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGameLibrary.Entities;
using MonoGameLibrary.Shapes;
using MonoGameLibrary.Misc;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Managers;

namespace KingdomLike.Entities;

public class Floor : Entity
{
    public Floor(string name) : base(name)
    {
        UsedBatch = ProjectBatchHandling.Instance.MainLayerBatch;
    }

    public override void Initialize()
    {
        base.Initialize();

        canCollide = true;
        canRender = true;
        collisionType = CollisionType.STATIC;

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

        sprite = RessourceManager.Instance.GetOrAddSprite("wall", atlas2);
    }
}
