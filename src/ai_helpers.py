"""A collection of helpers for executing entity AI."""

from __future__ import annotations

from typing import TYPE_CHECKING

import tcod.ecs
import tcod.path

from actions import BumpAction, MeleeAction, MoveAction
from components import AI, ActionDelay, Position, Stats
from constants import Tile

if TYPE_CHECKING:
    import random

    from gamemap import GameMap
    from messagelog import MessageLog


def confused_action(
    entity: tcod.ecs.Entity, gamemap: GameMap, rng: random.Random, message_log: MessageLog
) -> None:
    """Execute an action for a confused entity."""
    directories = [(x, y) for x in range(-1, 2) for y in range(-1, 2)]
    directory = rng.choice(directories)
    BumpAction(entity, directory, gamemap, rng, message_log).perform()
    entity.components[ActionDelay].ticks = 15


def wander_action(
    entity: tcod.ecs.Entity, gamemap: GameMap, rng: random.Random
) -> None:
    """Have an entity wander around randomly."""
    directories = [
        (x, y) for x in range(-1, 2) for y in range(-1, 2) if (x, y) != (0, 0)
    ]
    directory = rng.choice(directories)
    MoveAction(entity, directory, gamemap).perform()
    entity.components[ActionDelay].ticks = 15


def hostile_action(
    entity: tcod.ecs.Entity, gamemap: GameMap, rng: random.Random, message_log: MessageLog
) -> None:
    """Look for a valid target and wander if none found."""
    stats = entity.components[Stats]
    e_pos = entity.components[Position]
    path = entity.components[AI].path
    playerpos = gamemap.player.components[Position]
    dx = playerpos.x - e_pos.x
    dy = playerpos.y - e_pos.y
    distance = max(abs(dx), abs(dy))
    visible_tiles = gamemap.get_fov(e_pos.xy, int(stats.sight))
    if visible_tiles[playerpos.xy]:
        if distance <= 1:
            entity.components[ActionDelay].ticks = 15
            MeleeAction(entity, (dx, dy), gamemap, rng, message_log).perform()
            return
        graph = tcod.path.SimpleGraph(
            cost=gamemap.tiles[Tile.MOVEMENTCOST], cardinal=2, diagonal=3
        )
        pathfinder = tcod.path.Pathfinder(graph)
        pathfinder.add_root(playerpos.xy)
        path: list[tuple[int, int]] = pathfinder.path_from(e_pos.xy)[1:].tolist() # type: ignore

    if path:
        dest_x, dest_y = path.pop(0)
        MoveAction(entity, (dest_x - e_pos.x, dest_y - e_pos.y), gamemap).perform()
        entity.components[ActionDelay].ticks = 15
        return

    wander_action(entity, gamemap, rng)
    return
