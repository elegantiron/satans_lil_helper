using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;

namespace SatansLilHelper.EntityFactories;

internal sealed class ItemFactory
{
    private static readonly Lazy<ItemFactory> _instance = new(() => new ItemFactory());
    public static ItemFactory Instance => _instance.Value;
    public EntityStore Store => store;
    public Entity Torch => torch;

    private EntityStore store;
    private Entity torch,
        healthPotion;

    private ItemFactory()
    {
        store = new();
        torch = store.CreateEntity();
        torch.Add(
            new EntityName("Torch"),
            new ItemSlots(ItemType.Torch),
            Tags.Get<Equippable, Item>()
        );
        torch.AddRelation(new AbilityStat(AbilityID.LightRadius, 6m));

        healthPotion = store.CreateEntity();
        healthPotion.Add(
            new EntityName("Healing Potion"),
            new RandomEffect(ItemEffect.Heal, dice: 1, sides: 8, bonus: 2),
            Tags.Get<Activatable, Item>()
        );
    }
}
