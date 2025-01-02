from __future__ import annotations

from typing import TYPE_CHECKING

from actions import BumpAction, MoveAction, MeleeAction
from components import ActionDelay, Position, Stats, AI

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
    entity.components[ActionDelay].ticks = 15


def wander_action(
    entity: tcod.ecs.Entity, gamemap: GameMap, rng: random.Random
) -> None:
    dirs = [(x, y) for x in range(-1, 2) for y in range(-1, 2) if (x, y) != (0, 0)]
    dir = rng.choice(dirs)
    MoveAction(entity, dir, gamemap).perform()
    entity.components[ActionDelay].ticks = 15


def hostile_action(entity: tcod.ecs.Entity, gamemap: GameMap, rng: random.Random):
    stats = entity.components[Stats]
    e_pos = entity.components[Position]
    path = entity.components[AI].path
    playerpos = gamemap.player.components[Position]
    dx = playerpos.x - e_pos.x
    dy = playerpos.y - e_pos.y
    distance = max(abs(dx), abs(dy))
    visible_tiles = gamemap.get_fov(e_pos.xy, stats.sight)
    if visible_tiles[playerpos.xy]:
        if distance <= 1:
            return MeleeAction(entity, (dx, dy), gamemap, rng).perform()
        gamemap.pathfinder.clear()
        gamemap.pathfinder.add_root(playerpos.xy)
        path = gamemap.pathfinder.path_from(e_pos.xy)[1:].tolist()

    if path:
        dest_x, dest_y = path.pop(0)
        MoveAction(entity, (dest_x - e_pos.x, dest_y - e_pos.y), gamemap).perform()
        entity.components[ActionDelay].ticks = 15

    else:
        wander_action(entity, gamemap, rng)
