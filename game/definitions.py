from __future__ import annotations
from dataclasses import dataclass

from numpy import dtype

from game.constants import Sprites, TileDict


@dataclass
class Color:
    """A class to handle colors"""

    r: int
    g: int
    b: int
    a: int

    @property
    def rgb(self) -> tuple[int, int, int]:
        return self.r, self.g, self.b

    @property
    def rgba(self) -> tuple[int, int, int, int]:
        return *self.rgb, self.a


tile_dt = dtype(
    [
        (TileDict.Walkable, bool),
        (TileDict.Transparent, bool),
        (TileDict.Explored, bool),
        (TileDict.Visible, bool),
        (TileDict.Safe, bool),
        (TileDict.MovementCost, int),
        (TileDict.SpriteID, Sprites),
    ]
)
