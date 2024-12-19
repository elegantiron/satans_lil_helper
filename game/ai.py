from __future__ import annotations
from typing import TYPE_CHECKING

from game import actions
from game.components import EntityAI
from game.constants import DIRS, Status

if TYPE_CHECKING:
    import tcod.ecs
    from game.gameworld import GameWorld


def get_enemy_action(entity: tcod.ecs.Entity, world: GameWorld) -> actions.Action:
    match entity.components[EntityAI].type:
        case Status.Hostile:
            pass
        case Status.Confused | Status.Wandering:
            direction = world.rng.choice([*DIRS, (0, 0)])
            return actions.BumpAction(
                entity, direction, world.current_map, world.rng
            ).perform()
