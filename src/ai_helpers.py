from __future__ import annotations

from typing import TYPE_CHECKING

from actions import BumpAction, MoveAction
from components import ActionDelay

if TYPE_CHECKING:
    import random

    import tcod.ecs

    from gamemap import GameMap


def confused_action(
    entity: tcod.ecs.Entity, gamemap: GameMap, rng: random.Random
) -> None:
    dirs = [(x, y) for x in range(-1, 2) for y in range(-1, 2)]
    dir = rng.choice(dirs)
    BumpAction(entity, dir, gamemap, rng).perform()


def wander_action(
    entity: tcod.ecs.Entity, gamemap: GameMap, rng: random.Random
) -> None:
    dirs = [(x, y) for x in range(-1, 2) for y in range(-1, 2) if (x, y) != (0, 0)]
    dir = rng.choice(dirs)
    MoveAction(entity, dir, gamemap).perform()
    entity.components[ActionDelay].ticks = 15
