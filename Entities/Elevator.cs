using KingdomLike.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoGameLibrary;
using MonoGameLibrary.Entities;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Interfaces;
using MonoGameLibrary.Managers;
using MonoGameLibrary.Misc;

namespace KingdomLike.Entities;

public class Elevator : Entity, IInteractable
{
    private Vector2 destination;

    private Trigger trigger;

    public Elevator(string name) : base(name)
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

        sprite = RessourceManager.Instance.GetOrAddSprite("container", atlas2);
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
        interactor.SetPosition(destination);
    }

    public void SetDestination(Vector2 newDestination)
    {
        destination = newDestination;
    }
}
