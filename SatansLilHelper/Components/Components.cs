using Friflo.Engine.ECS;

namespace SatansLilHelper.Components;

[ComponentKey("action-delay")]
public struct ActionDelay : IComponent
{
    public int value;
}

[ComponentKey("location")]
public struct Location : IIndexedComponent<(int, int)>
{
    public int X,
        Y;

    public Location(int x, int y)
    {
        X = x;
        Y = y;
    }

    public (int, int) GetIndexedValue()
    {
        return (X, Y);
    }
}
