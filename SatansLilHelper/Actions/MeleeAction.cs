using System.Collections.Generic;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Components;
using SatansLilHelper.Content.Text;
using SatansLilHelper.EntityTags;
using SatansLilHelper.Exceptions;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

internal class MeleeAction : ActionWithDirection, IMessageSender
{
    protected byte[] preState,
        postState;
    protected MersenneTwister Twister;
    protected int Damage;
    protected bool IsKill;
    protected string TargetName;
    public List<LogMessage> Messages
    {
        get { return _messages; }
    }

    public MeleeAction(
        Entity entity,
        Point direction,
        BaseMap gameMap,
        MersenneTwister rng,
        bool isPlayer
    )
        : base(entity, direction, gameMap, isPlayer)
    {
        if (!_isBlocked || _target == null)
            throw new MissingTargetException();
        TargetName = _target?.GetComponent<EntityName>().value;
        Twister = rng;
        preState = rng.GetState();

        Damage = EntityCalcs.GetDamage(Twister, _entity, (Entity)_target);
        _messages.Add(
            new(
                string.Format(GameStrings.PlayerAttack, TargetName, Damage),
                Constants.Colors.PlayerAttack
            )
        );
        if (Damage >= _target?.GetRelation<ResourceStat, ResourceID>(ResourceID.Health).Cur)
        {
            IsKill = true;
            _messages.Add(
                new(
                    string.Format(GameStrings.EnemyDeath, TargetName),
                    Constants.Colors.PlayerAttack
                )
            );
            _messages.Add(
                new(string.Format(GameStrings.GainExperience, 5), Constants.Colors.AmericanRose)
            );
        }
        postState = rng.GetState();
        rng.SetState(preState);
    }

    public override void Perform()
    {
        base.Perform();
        Twister.SetState(postState);
        Entity entity = _target ?? default;
        ref ResourceStat entHealth = ref entity.GetRelation<ResourceStat, ResourceID>(
            ResourceID.Health
        );
        entHealth.Cur -= Damage;
        if (IsKill)
            entity.RemoveTag<Alive>();
    }

    public override void Rewind()
    {
        base.Rewind();
        Twister.SetState(preState);
        Entity entity = _target ?? default;
        ref ResourceStat entHealth = ref entity.GetRelation<ResourceStat, ResourceID>(
            ResourceID.Health
        );
        entHealth.Cur += Damage;
        if (IsKill)
            entity.AddTag<Alive>();
    }
}
