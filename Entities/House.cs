using KingdomLike.Rendering;
using Microsoft.Xna.Framework.Content;
using MonoGameLibrary.Entities;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Managers;

namespace KingdomLike.Entities;

public class House : Entity
{
    public House(string name) : base(name)
    {
        usedBatch = ProjectBatchHandling.Instance.MainLayerBatch;
    }

    public override void Initialize()
    {
        base.Initialize();

        canRender = true;
    }

    public override void LoadContent(ContentManager content)
    {
        base.LoadContent(content);

        // Create the texture atlas from the XML configuration file
        TextureAtlas atlas2 = RessourceManager.Instance.GetOrAddTextureAtlas("images/atlas-definition2.xml");

        sprite = RessourceManager.Instance.GetOrAddSprite("container", atlas2);
    }
}
