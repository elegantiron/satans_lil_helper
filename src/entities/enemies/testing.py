from __future__ import annotations

from typing import TYPE_CHECKING

import abilities
from components import AI, ActionDelay, Attack, Name, Position, Skills, Stats
from constants import AIType, EntityTags

if TYPE_CHECKING:
    from random import Random

    from tcod.ecs import Entity


def strong(entity: Entity, position: tuple[int, int], rng: Random) -> None:
    entity.components[Stats] = Stats(
        100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 0, 0, 1
    )
    entity.components[Attack] = Attack(2, 8)
    entity.components[Skills] = Skills(onetime=abilities.Abilities.DARK_VISION)
    entity.components[Position] = Position(
        x=position[0], y=position[1], sprite=":images:enemies/wolf32.png"
    )
    entity.components[AI] = AI(AIType.HOSTILE, AIType.HOSTILE)
    entity.components[ActionDelay] = ActionDelay(0)
    entity.components[Name] = Name(
        "forgotten beast", definite_article="the", indefinite_article="a"
    )
    entity.tags.add(EntityTags.HOSTILE)


def weak(entity: Entity, position: tuple[int, int], rng: Random) -> None:
    entity.components[Stats] = Stats(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 10, 1)
    entity.components[Attack] = Attack(1, 1)
    entity.components[Skills] = Skills(onetime=abilities.Abilities.DARK_VISION)
    entity.components[Position] = Position(
        x=position[0], y=position[1], sprite=":images:enemies/wolf32.png"
    )
    entity.components[AI] = AI(AIType.HOSTILE, AIType.HOSTILE)
    entity.components[ActionDelay] = ActionDelay(0)
    entity.components[Name] = Name(
        "forgotten beast", definite_article="the", indefinite_article="a"
    )
    entity.tags.add(EntityTags.HOSTILE)
