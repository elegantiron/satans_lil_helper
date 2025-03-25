using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Types;

#nullable enable
public struct ActionResult()
{
    public List<LogMessage>? Messages = null;
    public MovementResult? MovementResult = null;
    public List<EntityCreationResult>? EntityCreationResults = null;
    public List<AttackResult>? AttackResults = null;

    public ActionResult(MovementResult movementResult, List<LogMessage>? messages = null)
        : this()
    {
        MovementResult = movementResult;
        Messages = messages;
    }
}
