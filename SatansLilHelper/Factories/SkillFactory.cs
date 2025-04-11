using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;

namespace SatansLilHelper.Factories;

internal sealed class SkillFactory
{
    private static readonly Lazy<SkillFactory> _instance = new(() => new SkillFactory());

    public static SkillFactory Default => _instance.Value;
    private EntityBatch charge,
        shieldUp;
    public EntityBatch Charge => charge;
    public EntityBatch ShieldUp => shieldUp;

    private SkillFactory()
    {
        charge = new();
        charge.Add(new EntityName("Charge"));
        charge.Add(new Targetable(3, 0));
        charge.AddTag<Activatable>();
        charge.AddTag<DamagesInterruptor>();
        charge.AddTag<Interruptible>();
        charge.AddTag<MovesActor>();
        charge.AddTag<Skill>();

        shieldUp = new();
        shieldUp.Add(new EntityName("Shield Up"));
        shieldUp.AddTag<Activatable>();
        shieldUp.AddTag<Skill>();
    }
}
