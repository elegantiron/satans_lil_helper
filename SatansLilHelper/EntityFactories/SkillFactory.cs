using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;

namespace SatansLilHelper.EntityFactories;

internal sealed class SkillFactory
{
    private static readonly Lazy<SkillFactory> _instance = new(() => new SkillFactory());
    public static SkillFactory Instance => _instance.Value;
    public EntityStore Store => store;
    public Entity Charge => charge;
    public Entity ShieldUp => shieldUp;

    private EntityStore store;
    private Entity charge,
        shieldUp;

    private SkillFactory()
    {
        store = new();
        charge = store.CreateEntity();
        charge.Add(
            new EntityName("Charge"),
            new Targetable(3, 0),
            Tags.Get<Activatable, DamagesInterruptor, Interruptible, MovesActor, Skill>()
        );

        shieldUp = store.CreateEntity();
        shieldUp.Add(new EntityName("Shield Up"), Tags.Get<Activatable, Skill>());
    }
}
