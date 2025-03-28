using System;
using System.Collections.Generic;
using System.Linq;
using SatansLilHelper.Constants;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Interfaces;

public interface IMessageSender
{
    List<LogMessage> Messages { get; }
    void SendMessages()
    {
        foreach (LogMessage logMessage in Messages)
            EventBus.Send(Events.AddLogMessage, logMessage);
    }

    void RetractMessages()
    {
        foreach (LogMessage logMessage in Messages.AsReadOnly().Reverse())
            EventBus.Send(Events.PruneLogMessage, logMessage);
    }
}
