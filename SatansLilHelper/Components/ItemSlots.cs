using Friflo.Engine.ECS;
using Friflo.Json.Fliox;
using SatansLilHelper.Constants;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Components;

[ComponentKey("item-slots")]
public struct ItemSlots(ItemType types) : IComponent
{
    [Serialize]
    public ItemType Types = types;
}
