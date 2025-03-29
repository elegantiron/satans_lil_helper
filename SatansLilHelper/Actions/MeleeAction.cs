using System.Collections.Generic;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.EntityTags;
using SatansLilHelper.Exceptions;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Properties;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

#nullable enable
internal class MeleeAction : ActionWithDirection, IMessageSender, IAttackAction
{
    protected byte[] _preState,
        _postState;
    protected IRandom _twister;
    protected uint _damage;
    protected bool _isKill;
    protected string _targetName;
    public List<LogMessage> Messages
    {
        get { return _messages; }
    }

    public MeleeAction(Entity entity, Point direction, BaseMap gameMap, IRandom rng, bool isPlayer)
        : base(entity, direction, gameMap, isPlayer)
    {
        if (_isBlocked && _target is Entity targetEnt)
        {
            _targetName = targetEnt.GetComponent<EntityName>().value;
            _twister = rng;
            _preState = _twister.GetState();

            _damage = EntityCalcs.GetDamage(_twister, _entity, targetEnt);
            _messages.Add(
                new(
                    string.Format(GameStrings.PlayerAttack, _targetName, _damage),
                    Constants.Colors.PlayerAttack
                )
            );
            if (_damage >= targetEnt.GetRelation<AbilityStat, AbilityID>(AbilityID.Health).Cur)
            {
                _isKill = true;
                _messages.Add(
                    new(
                        string.Format(GameStrings.EnemyDeath, _targetName),
                        Constants.Colors.PlayerAttack
                    )
                );
                _messages.Add(
                    new(string.Format(GameStrings.GainExperience, 5), Constants.Colors.AmericanRose)
                );
            }
            _postState = _twister.GetState();
        }
        else
            throw new MissingTargetException();
    }

    public override void Perform()
    {
        if (_target is Entity entity)
        {
            ref AbilityStat entHealth = ref entity.GetRelation<AbilityStat, AbilityID>(
                AbilityID.Health
            );
            entHealth.Cur -= _damage;
            if (_isKill)
                entity.RemoveTag<Alive>();
            _twister.SetState(_postState);
            base.Perform();
        }
    }

    public override void Rewind()
    {
        if (_target is Entity entity)
        {
            ref AbilityStat entHealth = ref entity.GetRelation<AbilityStat, AbilityID>(
                AbilityID.Health
            );
            entHealth.Cur += _damage;
            if (_isKill)
                entity.AddTag<Alive>();
            _twister.SetState(_preState);
            base.Rewind();
        }
    }
}
