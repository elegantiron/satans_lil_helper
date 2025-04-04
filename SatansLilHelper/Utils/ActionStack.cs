using System.Collections.Generic;
using Friflo.Engine.ECS;
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

    public void AddAction(PlayerTurn action)
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

    public bool IsPlayerTurn
    {
        get { return history.TryPeek(out IAction action) && action is PlayerTurn; }
    }

    public bool HasActions
    {
        get { return history.Count > 0; }
    }

    public bool GetPlayerTurn(Entity player, out PlayerTurn playerTurn)
    {
        playerTurn = new(player);
        if (!history.TryPeek(out IAction result))
        {
            return false;
        }
        if (result is PlayerTurn turn)
        {
            playerTurn = turn;
            history.Pop();
            return true;
        }
        return false;
    }
}
