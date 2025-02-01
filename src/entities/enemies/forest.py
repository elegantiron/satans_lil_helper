"""Forest entities"""

from __future__ import annotations

from typing import TYPE_CHECKING

import abilities
from components import AI, ActionDelay, Attack, Name, Position, Skills, Stats
from constants import AIType, EntityTags

if TYPE_CHECKING:
    from random import Random

    import tcod.ecs


def wolf(entity: tcod.ecs.Entity, position: tuple[int, int], rng: Random) -> None:
    """Give an entity a wolf's properties"""
    entity.components[Stats] = Stats(
        rng.randint(1, 8) + 16,
        strength=2,
        pdef=5,
        crit=1,
        sight=8,
    )
    entity.components[Attack] = Attack(1, 6)
    entity.components[Skills] = Skills(
        onetime=abilities.Abilities.HOWL
        | abilities.Abilities.DARK_VISION
        | abilities.Abilities.GNAW
        | abilities.Abilities.PACK_TACTICS
    )
    entity.components[Position] = Position(
        x=position[0], y=position[1], sprite=":images:enemies/wolf32.png"
    )
    entity.components[AI] = AI(AIType.HOSTILE, AIType.HOSTILE)
    entity.components[ActionDelay] = ActionDelay(rng.randint(1, 15))
    entity.components[Name] = Name("wolf")
    entity.tags.add(EntityTags.HOSTILE)
