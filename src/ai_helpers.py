from __future__ import annotations

from typing import TYPE_CHECKING

from actions import BumpAction, MoveAction
from components import ActionDelay, Position, Stats

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


def hostile_action(entity: tcod.ecs.Entity, gamemap: GameMap, rng: random.Random):
    stats = entity.components[Stats]
    e_pos = entity.components[Position]
    playerpos = gamemap.player.components[Position]
    visible_tiles = gamemap.get_fov(e_pos.xy, stats.sight)
    if visible_tiles[playerpos.xy]:
        gamemap.pathfinder.clear()
        gamemap.pathfinder.add_root(playerpos.xy)
        tile = gamemap.pathfinder.path_from(e_pos.xy)[1]
        direction = abs(e_pos.x - tile[0]), abs(e_pos.y - tile[1])
        BumpAction(entity, direction, gamemap, rng)
    else:
        wander_action(entity, gamemap, rng)
