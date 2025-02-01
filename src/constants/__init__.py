"""Constants"""

from enum import StrEnum
from typing import Final

from . import keylists
from .colors import Color
from .strings import Strings
from .tags import AIType, EntityTags, ItemType

__all__ = [
    "Strings",
    "keylists",
    "Color",
    "EntityTags",
    "AIType",
    "ItemType",
]

TILE_SIZE: Final = 32

LINE_SPACING: Final = 2


class Tile(StrEnum):
    """Tile attributes"""

    WALKABLE = "walkable"
    TRANSPARENT = "transparent"
    EXPLORED = "explored"
    VISIBLE = "visible"
    SAFE = "safe"
    MOVEMENTCOST = "movement cost"
    SPRITEPATH = "sprite path"
