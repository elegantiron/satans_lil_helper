from __future__ import annotations

import numpy as np

from constants import SPRITES

floor = 0
wall = 0


tile_dt = np.dtype(
    [
        ("walkable", bool),
        ("transparent", bool),
        ("sprite_id", SPRITES),
    ]
)

new_tile_dt = np.dtype(
    [
        ("walkable", bool),
        ("transparent", bool),
        ("explored", bool),
        ("sprite_id", SPRITES),
    ]
)


def new_tile(
    *,
    walkable: int,
    transparent: int,
    sprite_id: SPRITES,
):
    return np.array((walkable, transparent, sprite_id), dtype=tile_dt)


forest_floor = new_tile(
    walkable=True,
    transparent=True,
    sprite_id=SPRITES.FOREST_FLOOR,
)

forest_wall = new_tile(walkable=False, transparent=False, sprite_id=SPRITES.FOREST_WALL)
