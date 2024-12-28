from __future__ import annotations

from random import Random
from typing import TYPE_CHECKING

from components import Attack,  Specials, Stats
from constants import  ActiveAbilities, PassiveAbilities, SpecialAttacks

if TYPE_CHECKING:
    import tcod.ecs


def Wolf(entity: tcod.ecs.Entity, rng: Random):
    entity.components[Stats] = Stats(
        rng.randint(1, 8) + 16,
        strength=2,
        pdef=5,
        crit=1,
        sight=8,
    )
    entity.components[Attack] = Attack(1, 6)
    entity.components[Specials] = Specials(
        attacks=[SpecialAttacks.Gnaw],
        passives=[PassiveAbilities.PackTactics],
        skills=[ActiveAbilities.Howl],
    )
