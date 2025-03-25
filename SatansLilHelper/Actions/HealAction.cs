using System;
using System.Collections.Generic;
using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Content.Text;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Actions;

#nullable enable
internal class HealAction : IAction, IMessageSender
{
    private Entity _target;
    private int _healAmount;
    private List<LogMessage> _messages = [];

    public HealAction(Entity target, int healAmount)
    {
        _target = target;
        _healAmount = Math.Min(
            healAmount,
            EntityCalcs.GetStat(_target, ResourceID.Health)
                - _target.GetRelation<ResourceStat, ResourceID>(ResourceID.Health).Cur
        );
        _messages.Add(
            new LogMessage(
                string.Format(GameStrings.HealPotionMessage, _healAmount),
                Constants.Colors.PlayerHeal
            )
        );
    }

    public List<LogMessage> Messages
    {
        get { return _messages; }
    }
    public Entity Entity
    {
        get { return _target; }
    }

    public void Perform()
    {
        (this as IMessageSender).SendMessages();
        _target.GetRelation<ResourceStat, ResourceID>(ResourceID.Health).Cur += _healAmount;
    }

    public void Rewind()
    {
        (this as IMessageSender).RetractMessages();
        _target.GetRelation<ResourceStat, ResourceID>(ResourceID.Health).Cur -= _healAmount;
    }
}
