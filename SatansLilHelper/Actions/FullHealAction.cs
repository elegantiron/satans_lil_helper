using System.Collections.Generic;

using Friflo.Engine.ECS;

using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Properties;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Actions;

#nullable enable
internal class FullHealAction : IAction, IMessageSender
{
    private Entity _target;
    private int _healAmount;
    private List<LogMessage> _messages = [];

    public FullHealAction(Entity target)
    {
        _target = target;
        _healAmount =
            EntityCalcs.GetStat(_target, AbilityID.Health)
            - _target.GetRelation<AbilityStat, AbilityID>(AbilityID.Health).Cur;
        _messages.Add(new LogMessage(GameStrings.FullHeal, Constants.Colors.PlayerHeal));
    }

    public List<LogMessage> Messages => _messages;
    public bool Successful => true;
    public Entity? Entity => _target;

    public void Perform()
    {
        (this as IMessageSender).SendMessages();
        _target.GetRelation<AbilityStat, AbilityID>(AbilityID.Health).Cur += _healAmount;
    }

    public void Rewind()
    {
        (this as IMessageSender).RetractMessages();
        _target.GetRelation<AbilityStat, AbilityID>(AbilityID.Health).Cur -= _healAmount;
    }
}
