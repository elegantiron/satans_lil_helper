using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Serialization;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;

namespace SatansLilHelper.Types;

internal class Bestiary
{
    private readonly Dictionary<EnemyType, IKill> _kills;

    public Bestiary()
    {
        _kills = [];
    }

    public void AddKill(EnemyType enemyType, bool isAlpha = false)
    {
        if (_kills.TryGetValue(enemyType, out IKill? kill))
            kill.AddKill(isAlpha);
    }

    public void RemoveKill(EnemyType enemyType, bool isAlpha = false)
    {
        if (_kills.TryGetValue(enemyType, out IKill? kill))
            kill.RemoveKill(isAlpha);
    }

    public bool GetStats(EnemyType enemyType, [MaybeNullWhen(false)] out IKill kill)
    {
        return _kills.TryGetValue(enemyType, out kill);
    }

    private class Kill : IKill
    {
        private int _alpha,
            _normal;
        public int Alpha => _alpha;
        public int Normal => _normal;

        public void AddKill(bool isAlpha = false)
        {
            if (isAlpha)
                _alpha++;
            else
                _normal++;
        }

        public void RemoveKill(bool isAlpha = false)
        {
            if (isAlpha && _alpha > 0)
                _alpha--;
            else if (_normal > 0)
                _normal--;
        }
    }
}
