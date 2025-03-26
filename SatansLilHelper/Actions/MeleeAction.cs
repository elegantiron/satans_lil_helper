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

#nullable enable
internal class MeleeAction : ActionWithDirection, IMessageSender
{
    protected byte[] preState,
        postState;
    protected MersenneTwister Twister;
    protected uint Damage;
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
        if (_isBlocked && _target is Entity targetEnt)
        {
            TargetName = targetEnt.GetComponent<EntityName>().value;
            Twister = rng;
            preState = Twister.GetState();

            Damage = EntityCalcs.GetDamage(Twister, _entity, targetEnt);
            _messages.Add(
                new(
                    string.Format(GameStrings.PlayerAttack, TargetName, Damage),
                    Constants.Colors.PlayerAttack
                )
            );
            if (Damage >= targetEnt.GetRelation<ResourceStat, ResourceID>(ResourceID.Health).Cur)
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
            postState = Twister.GetState();
        }
        else
            throw new MissingTargetException();
    }

    public override void Perform()
    {
        if (_target is Entity entity)
        {
            ref ResourceStat entHealth = ref entity.GetRelation<ResourceStat, ResourceID>(
                ResourceID.Health
            );
            entHealth.Cur -= Damage;
            if (IsKill)
                entity.RemoveTag<Alive>();
            Twister.SetState(postState);
            base.Perform();
        }
    }

    public override void Rewind()
    {
        if (_target is Entity entity)
        {
            ref ResourceStat entHealth = ref entity.GetRelation<ResourceStat, ResourceID>(
                ResourceID.Health
            );
            entHealth.Cur += Damage;
            if (IsKill)
                entity.AddTag<Alive>();
            Twister.SetState(preState);
            base.Rewind();
        }
    }
}
