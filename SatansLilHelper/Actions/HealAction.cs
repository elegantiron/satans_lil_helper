using System;
using System.Collections.Generic;
using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Actions;

internal class HealAction : IAction, IMessageSender
{
    private Entity _actor,
        _item;
    private List<LogMessage> _messages;
    private float _healAmount;
    private bool _successful = true;

    public HealAction(Entity actor, Entity item)
    {
        _actor = actor;
        _item = item;
        AbilityStat actorHealth = _actor.GetRelation<AbilityStat, AbilityID>(AbilityID.Health);
        Effect itemEffect = _item.GetComponent<Effect>();
        float missingHealth = EntityCalcs.GetStat(_actor, AbilityID.Health) - actorHealth.Cur;
        if (missingHealth == 0)
            _successful = false;

        if (missingHealth > itemEffect.Value)
            _healAmount = itemEffect.Value;
        else
            _healAmount = missingHealth;
        string text = String.Format(Properties.GameStrings.HealPotionMessage, _healAmount);
        _messages = [new LogMessage(text, Colors.PlayerHeal)];
    }

    public void Perform()
    {
        (this as IMessageSender).SendMessages();
        if (_successful)
        {
            _actor.GetRelation<AbilityStat, AbilityID>(AbilityID.Health).Cur += (uint)_healAmount;
            _item.Enabled = false;
        }
    }

    public void Rewind()
    {
        (this as IMessageSender).RetractMessages();
        if (_successful)
        {
            _actor.GetRelation<AbilityStat, AbilityID>(AbilityID.Health).Cur -= (uint)_healAmount;
            _item.Enabled = true;
        }
    }

    public Entity? Entity => _actor;
    public List<LogMessage> Messages => _messages;
}
