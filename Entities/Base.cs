using KingdomLike.Rendering;
using KingdomLike.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGameLibrary;
using MonoGameLibrary.Entities;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Interfaces;
using MonoGameLibrary.Managers;
using MonoGameLibrary.Misc;

namespace KingdomLike.Entities;

public class Base : Entity, IInteractable
{
    private Trigger trigger;

    private bool isSoldierBase = false;

    public Base(string name) : base(name)
    {
        usedBatch = ProjectBatchHandling.Instance.MainLayerBatch;
    }

    public override void Initialize()
    {
        base.Initialize();

        canRender = true;

        trigger = new Trigger(entityName + " Trigger");
        trigger.LoadContent(Core.Content);
        trigger.Initialize();
        trigger.SetTriggerSize(new Vector2(sprite.Width, sprite.Height));
        trigger.AttachTo(this);
        trigger.SetRelativePosition(0, 0);
        trigger.SetPosition(position);
        trigger.Register();
        trigger.SetLayer(10);
    }

    public override void LoadContent(ContentManager content)
    {
        base.LoadContent(content);

        // Create the texture atlas from the XML configuration file
        TextureAtlas atlas2 = RessourceManager.Instance.GetOrAddTextureAtlas("images/atlas-definition2.xml");

        sprite = RessourceManager.Instance.GetOrAddSprite("base", atlas2);
    }

    public bool CanBeInteracted()
    {
        return true;
    }

    public Entity GetEntity()
    {
        return this;
    }

    public void OnInteract(Entity interactor)
    {
        if (KingdomLikeGameManager.instance.gold >= 5)
        {
            if (!isSoldierBase)
            {
                Extortionist extortionist = new Extortionist("Extortionist");
                extortionist.LoadContent(Core.Content);
                extortionist.Initialize();
                extortionist.Register();
                extortionist.SetPosition(Position);
            }
            else
            {
                Soldier soldier = new Soldier("Soldier");
                soldier.LoadContent(Core.Content);
                soldier.Initialize();
                soldier.Register();
                soldier.SetPosition(Position);
            }
            KingdomLikeGameManager.instance.gold -= 5;
        }
    }

    public void SetIsSoldierBase()
    {
        isSoldierBase = true;
    }
}
