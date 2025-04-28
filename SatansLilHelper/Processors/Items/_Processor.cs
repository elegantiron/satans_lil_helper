using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friflo.Engine.ECS;
using SatansLilHelper.Constants;

namespace SatansLilHelper.Processors;

internal static partial class Items
{
    public static void Process(Entity item, Entity user)
    {
        if (item.Tags.Has<Equippable>())
            Equippable(item, user);
        else if (item.Tags.Has<Consumable>())
            Consumable(item, user);
        else
            Default(item);
    }

    private static void Default(Entity item) { }
}
