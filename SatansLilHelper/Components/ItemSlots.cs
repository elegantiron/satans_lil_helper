using Friflo.Engine.ECS;

using SatansLilHelper.Constants;

namespace SatansLilHelper.Components;

[ComponentKey("item-slots")]
public struct ItemSlots(ItemType types) : IComponent
{
    public ItemType Types = types;
}
