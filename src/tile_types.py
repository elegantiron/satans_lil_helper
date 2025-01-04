"""Types of tiles"""

from __future__ import annotations

import numpy as np

from constants import Tile

tile_dt = np.dtype(
    [
        (Tile.WALKABLE, bool),
        (Tile.TRANSPARENT, bool),
        (Tile.EXPLORED, bool),
        (Tile.VISIBLE, bool),
        (Tile.SAFE, bool),
        (Tile.MOVEMENTCOST, np.int8),
        (Tile.SPRITEPATH, np.str_),
    ]
)


def new_tile(
    *,
    walkable: bool = True,
    transparent: bool = True,
    explored: bool = False,
    visible: bool = False,
    safe: bool = False,
    movementcost: int = 1,
    sprite_path: str,
) -> np.ndarray:
    """Helper to make new tiles"""
    return np.array(
        (walkable, transparent, explored, visible, safe, movementcost, sprite_path),
        dtype=tile_dt,
    )


ForestFloor = new_tile(
    walkable=True,
    transparent=True,
    explored=False,
    visible=False,
    safe=False,
    movementcost=1,
    sprite_path=":images:tiles/forest/floor/000.png",
)

ForestWall = new_tile(
    walkable=False,
    transparent=False,
    explored=False,
    visible=False,
    safe=False,
    movementcost=0,
    sprite_path=":images:tiles/forest/wall/000.png",
)
