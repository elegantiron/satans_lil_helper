using System;
using System.Collections.Generic;
using Friflo.Engine.ECS;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;

namespace SatansLilHelper.Actions;

#nullable enable
internal class HealAction : IAction, IMessageSender
{
    private Entity _entity;
    private int _healAmount;
    private List<LogMessage> _messages;

    // nullable member fields
    private byte[]? _preState,
        _postState;
    public Entity Entity
    {
        get { return _entity; }
    }

    public void Perform()
    {
        IMessageSender.SendMessages(_messages);
    }

    public void Rewind()
    {
        RetractMessages(_messages);
    }
}
