using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;

namespace SatansLilHelper.EntityFactories;

internal sealed class EnemyFactory
{
    private readonly Lazy<EnemyFactory> _instance = new(() => new EnemyFactory());
    public EnemyFactory Instance => _instance.Value;
    public EntityStore Store => _store;
    public Entity Wolf => _wolf;
    private EntityStore _store;
    private Entity _wolf;

    private EnemyFactory()
    {
        _store = new();

        _wolf = _store.CreateEntity();
        _wolf.Add(
            new EntityName("wolf"),
            new TextureIndex(Constants.TextureID.Wolf),
            Tags.Get<Actor, Alive, Blocking, Hostile, Visible>()
        );
        _wolf.AddTags(Tags.Get<Nocturnal>());
        _wolf.AddRelation(new AbilityStat(AbilityID.Strength, 2));
        _wolf.AddRelation(new AbilityStat(AbilityID.PhysicalDefense, 5));
        _wolf.AddRelation(new AbilityStat(AbilityID.Crit, 1));
        _wolf.AddRelation(new AbilityStat(AbilityID.Speed, 3));
        _wolf.AddRelation(new AbilityStat(AbilityID.Vision, 8));
    }
}
