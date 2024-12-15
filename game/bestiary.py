from __future__ import annotations

from game.constants import EnemyType

kills = int
alpha_kills = int


class _MurderStats:
    def __init__(self, kills, alpha_kills):
        self.kills = 0
        self.alpha_kills = 0

    def add_kill(self, alpha: bool = False):
        if alpha:
            self.alpha_kills += 1
        else:
            self.kills += 1


class Bestiary:
    murders: dict[EnemyType, _MurderStats]

    def add_kill(self, type: EnemyType, alpha: bool = False):
        stats = self.murders.get(type)
        if stats is not None:
            stats.add_kill(alpha)
        else:
            self.murders.update({type: _MurderStats()})
            self.murders[type].add_kill(alpha)
