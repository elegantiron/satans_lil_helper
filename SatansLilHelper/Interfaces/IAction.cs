using Friflo.Engine.ECS;

namespace SatansLilHelper.Interfaces;

internal interface IAction
{
    void Perform();
    void Rewind();
    bool Successful { get; }
    Entity? Entity { get; }
}
