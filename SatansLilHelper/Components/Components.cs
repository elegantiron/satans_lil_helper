using Friflo.Engine.ECS;

namespace SatansLilHelper.Components;

[ComponentKey("action-delay")]
public struct ActionDelay : IComponent
{
    public int value;
}

[ComponentKey("location")]
public struct Location(int x, int y) : IIndexedComponent<(int, int)>
{
    public int X = x,
        Y = y;

    public readonly (int, int) GetIndexedValue()
    {
        return (X, Y);
    }
}
