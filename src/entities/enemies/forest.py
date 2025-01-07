"""Forest entities"""

from __future__ import annotations

from random import Random
from typing import TYPE_CHECKING

from components import AI, ActionDelay, Attack, Name, Position, Specials, Stats
from constants import AIType, EntityTags, abilities

if TYPE_CHECKING:
    import tcod.ecs


def wolf(*, position: tuple[int, int], entity: tcod.ecs.Entity, rng: Random):
    """Give an entity a wolf's properties"""
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
            attacks=abilities.Enemies.GNAW,
            passives=abilities.Enemies.PACK_TACTICS,
            skills=abilities.Enemies.HOWL,
        ),
        Position: Position(
            x=position[0], y=position[1], sprite=":images:enemies/wolf32.png"
        ),
        AI: AI(AIType.HOSTILE, AIType.HOSTILE),
        ActionDelay: ActionDelay(rng.randint(1, 15)),
        Name: Name("wolf"),
    }
    entity.tags.add(EntityTags.HOSTILE)
