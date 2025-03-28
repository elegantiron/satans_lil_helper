using System.Collections.Generic;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;

namespace SatansLilHelper.Utils;

internal class ActionStack
{
    private Stack<IAction> history = new(),
        future = new();

    public void AddAction(IAction action)
    {
        future.Clear();
        action.Perform();
        history.Push(action);
    }

    public void AddAction(EntityTurn action)
    {
        future.Clear();
        history.Push(action);
    }

    public void Rewind()
    {
        if (history.Count == 0)
            return;
        IAction action = history.Pop();
        action.Rewind();
        future.Push(action);
    }

    public void Replay()
    {
        if (future.Count == 0)
            return;
        IAction action = future.Pop();
        action.Perform();
        history.Push(action);
    }
}
