using System.Collections.Generic;

namespace SatansLilHelper.Types;

#nullable enable
public struct ActionResult()
{
    public List<LogMessage>? Messages = null;
    public MovementResult? MovementResult = null;
    public List<EntityCreationResult>? EntityCreationResults = null;
    public List<AttackResult>? AttackResults = null;

    public ActionResult(List<LogMessage>? messages)
        : this()
    {
        Messages = messages;
    }

    public ActionResult(MovementResult movementResult, List<LogMessage>? messages = null)
        : this(messages)
    {
        MovementResult = movementResult;
    }

    public ActionResult(AttackResult attackResult, List<LogMessage>? messages = null)
        : this(messages)
    {
        AttackResults = [];
        AttackResults.Add(attackResult);
    }

    public ActionResult(List<AttackResult> attackResults, List<LogMessage>? messages = null)
        : this(messages)
    {
        AttackResults = attackResults;
    }
}
