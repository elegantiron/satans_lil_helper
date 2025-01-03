from __future__ import annotations

from random import Random
from typing import TYPE_CHECKING

from components import AI, ActionDelay, Attack, Name, Position, Specials, Stats
from constants import ActiveAbilities, AIType, PassiveAbilities, SpecialAttacks

if TYPE_CHECKING:
    import tcod.ecs


def Wolf(*, position: tuple[int, int], entity: tcod.ecs.Entity, rng: Random):
    entity.components |= {
        Stats: Stats(
            rng.randint(1, 8) + 16,
            strength=2,
            pdef=5,
            crit=1,
            sight=8,
        ),
        Attack: Attack(1, 6),
        Specials: Specials(
            attacks=[SpecialAttacks.Gnaw],
            passives=[PassiveAbilities.PackTactics],
            skills=[ActiveAbilities.Howl],
        ),
        Position: Position(
            x=position[0], y=position[1], sprite=":images:enemies/wolf32.png"
        ),
        AI: AI(AIType.Hostile, AIType.Hostile),
        ActionDelay: ActionDelay(rng.randint(1, 15)),
        Name: Name("wolf"),
    }
