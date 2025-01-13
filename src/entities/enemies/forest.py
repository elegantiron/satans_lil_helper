"""Forest entities"""

from __future__ import annotations

from typing import TYPE_CHECKING

from components import AI, ActionDelay, Attack, Name, Position, Skills, Stats
from constants import AIType, EntityTags, abilities

if TYPE_CHECKING:
    from random import Random

    import tcod.ecs


def wolf(*, position: tuple[int, int], entity: tcod.ecs.Entity, rng: Random) -> None:
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
        Skills: Skills(
            onetime=abilities.Abilities.HOWL
            | abilities.Abilities.DARK_VISION
            | abilities.Abilities.GNAW
            | abilities.Abilities.PACK_TACTICS
        ),
        Position: Position(
            x=position[0], y=position[1], sprite=":images:enemies/wolf32.png"
        ),
        AI: AI(AIType.HOSTILE, AIType.HOSTILE),
        ActionDelay: ActionDelay(rng.randint(1, 15)),
        Name: Name("wolf"),
    }
    entity.tags.add(EntityTags.HOSTILE)
