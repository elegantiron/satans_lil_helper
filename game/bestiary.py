from __future__ import annotations
from typing import TYPE_CHECKING


from game.constants import EnemyType, FontDict

if TYPE_CHECKING:
    from pygame import Surface
    import pygame.freetype as freetype

kills = int
alpha_kills = int


class _MurderStats:
    def __init__(self, kills=0, alpha_kills=0):
        self.kills = kills
        self.alpha_kills = alpha_kills

    def add_kill(self, alpha: bool = False):
        if alpha:
            self.alpha_kills += 1
        else:
            self.kills += 1

    @property
    def total_kills(self) -> int:
        return self.kills + self.alpha_kills


class Bestiary:
    def __init__(self):
        self.murders = {EnemyType.Wolf: _MurderStats()}

    def add_kill(self, type: EnemyType, alpha: bool = False):
        stats = self.murders.get(type)
        if stats is not None:
            stats.add_kill(alpha)
        else:
            self.murders.update({type: _MurderStats()})
            self.murders[type].add_kill(alpha)

    def render(self, surface: Surface, fonts: dict[FontDict, freetype.Font]):
        # TODO: Render the bestiary
        pass
