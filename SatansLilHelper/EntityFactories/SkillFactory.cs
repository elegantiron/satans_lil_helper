using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friflo.Engine.ECS;
using SatansLilHelper.Constants;

namespace SatansLilHelper.EntityFactories;

internal sealed class SkillFactory
{
    private static readonly Lazy<SkillFactory> _instance = new(() => new SkillFactory());
    public static SkillFactory Instance => _instance.Value;

    public Entity Charge => skills[SkillID.Charge];

    private EntityStore store;
    private Dictionary<SkillID, Entity> skills;

    private SkillFactory()
    {
        store = new();
        skills = new()
        {
            { SkillID.Charge, store.CreateEntity() },
            { SkillID.ShieldUp, store.CreateEntity() },
        };
        Entities.Skills.Charge(skills[SkillID.Charge]);
    }
}
