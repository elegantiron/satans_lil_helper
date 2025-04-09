using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

internal class PickupItemAction : IAction, IMessageSender, IFreeAction
{
    private Entity _actor,
        _item;
    private List<LogMessage> _messages;
    private bool _successful = false;
    private Location _itemLocation;

    public PickupItemAction(Entity actor)
    {
        _actor = actor;
        _messages = [];
        Location actLoc = _actor.GetComponent<Location>();
        ArchetypeQuery<Location> locQuery = _actor
            .Store.Query<Location>()
            .HasValue<Location, (int, int)>((actLoc.X, actLoc.Y))
            .AllTags(Tags.Get<Item>());
        if (locQuery.Count == 0)
            _messages.Add(new LogMessage(Properties.GameStrings.NoItemToGrab, Colors.Impossible));
        else
        {
            _item = locQuery.ToEntityList()[0];
            _itemLocation = _item.GetComponent<Location>();
            _successful = true;
        }
    }

    public Entity? Entity => _actor;
    public bool Successful => _successful;

    public List<LogMessage> Messages => _messages;

    public void Perform()
    {
        (this as IMessageSender).SendMessages();
        if (_successful)
        {
            _item.AddComponent(new Inventory(_actor));
            _item.RemoveComponent<Location>();
        }
    }

    public void Rewind()
    {
        (this as IMessageSender).RetractMessages();
        if (_successful)
        {
            _item.RemoveComponent<Inventory>();
            _item.AddComponent(_itemLocation);
        }
    }
}
