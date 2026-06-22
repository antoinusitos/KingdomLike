using KingdomLike.Rendering;
using Microsoft.Xna.Framework.Content;
using MonoGameLibrary.Entities;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Managers;

namespace KingdomLike.Entities
{
    public class InteractionPlayerFeedback : Entity
    {
        public InteractionPlayerFeedback(string name) : base(name)
        {
            UsedBatch = ProjectBatchHandling.Instance.MainLayerBatch;
            canUpdate = true;
            canRender = true;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            TextureAtlas _atlas3 = RessourceManager.Instance.GetOrAddTextureAtlas("images/atlas-definition3.xml");

            sprite = RessourceManager.Instance.GetOrAddSprite("x", _atlas3);
        }
    }
}
