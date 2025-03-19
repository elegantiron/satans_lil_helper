using System.Collections.Generic;
using SatansLilHelper.Actions;

namespace SatansLilHelper.Utils;

public class ActionStack
{
    private Stack<BaseAction> history = new(),
        future = new();

    public void AddAction(BaseAction action)
    {
        future.Clear();
        action.Perform();
        history.Push(action);
    }

    public void Rewind()
    {
        if (history.Count == 0)
            return;
        BaseAction action = history.Pop();
        action.Rewind();
        future.Push(action);
    }

    public void Replay()
    {
        if (future.Count == 0)
            return;
        BaseAction action = future.Pop();
        action.Perform();
        history.Push(action);
    }
}
