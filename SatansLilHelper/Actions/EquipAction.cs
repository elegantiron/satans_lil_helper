using System.Collections.Generic;
using Friflo.Engine.ECS;
using SatansLilHelper.Constants;
using SatansLilHelper.ECSComponents;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Properties;
using SatansLilHelper.Types;

namespace SatansLilHelper.Actions;

#nullable enable
internal class EquipAction : IAction, IFreeAction, IMessageSender
{
    private Entity _actor;
    private Entity? _oldItem,
        _item;
    private List<LogMessage> _messages;
    private bool _successful = false;

    public EquipAction(Entity actor, Entity item)
    {
        _actor = actor;
        _item = item;
        _messages = [];
        if (!_actor.TryGetComponent<ItemSlots>(out ItemSlots actorSlots))
            throw new Exceptions.MissingComponentException();

        if (!item.TryGetComponent<ItemSlots>(out ItemSlots itemTypes))
            throw new Exceptions.MissingComponentException();

        if (!actorSlots.Types.HasFlag(itemTypes.Types))
            _messages.Add(new(GameStrings.CantUseItem, Colors.Impossible));
        else
            _successful = true;
        EntityLinks<Equipper> equipment = _actor.GetIncomingLinks<Equipper>();
        foreach (EntityLink<Equipper> link in equipment)
        {
            ItemType equippedType = link.Entity.GetComponent<ItemSlots>().Types;
            if (equippedType.HasFlag(itemTypes.Types))
                _oldItem = link.Entity;
        }
        if (_oldItem == item)
            _item = null;
    }

    public Entity? Entity => _actor;
    public bool Successful => _successful;

    public List<LogMessage> Messages => _messages;

    public void Perform()
    {
        if (_messages.Count > 0)
            (this as IMessageSender).SendMessages();
        if (!_successful)
            return;
        if (_oldItem is Entity oldItem)
            oldItem.RemoveComponent<Equipper>();
        if (_item is Entity item)
            item.AddComponent(new Equipper(_actor));
    }

    public void Rewind()
    {
        if (_messages.Count > 0)
            (this as IMessageSender).RetractMessages();
        if (!_successful)
            return;
        if (_item is Entity item)
            item.RemoveComponent<Equipper>();
        if (_oldItem is Entity oldItem)
            oldItem.AddComponent(new Equipper(_actor));
    }
}
