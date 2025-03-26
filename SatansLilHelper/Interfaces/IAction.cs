using Friflo.Engine.ECS;

namespace SatansLilHelper.Interfaces;

internal interface IAction
{
    void Perform();
    void Rewind();

    Entity? Entity { get; }
}
