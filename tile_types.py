import numpy as np
from constants import TILES

tile_dt = np.dtype([("walkable", bool), ("transparent", bool), ("sprite", "1B")])


def new_tile(
    *,
    walkable: int,
    transparent: int,
    sprite: int,
) -> np.ndarray:
    """Helper function for defining individual tile types."""
    return np.array((walkable, transparent, sprite), dtype=tile_dt)


floor = new_tile(
    walkable=True,
    transparent=True,
    sprite=TILES.FOREST_FLOOR,
)

wall = new_tile(
    walkable=False,
    transparent=False,
    sprite=TILES.FOREST_WALL,
)

down_stairs = new_tile(
    walkable=True,
    transparent=True,
    sprite=TILES.DOWN_STAIRS,
)
