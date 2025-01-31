"""Constants"""

from enum import StrEnum

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

TILE_SIZE = 32

LINE_SPACING = 2


class Tile(StrEnum):
    """Tile attributes"""

    WALKABLE = "walkable"
    TRANSPARENT = "transparent"
    EXPLORED = "explored"
    VISIBLE = "visible"
    SAFE = "safe"
    MOVEMENTCOST = "movement cost"
    SPRITEPATH = "sprite path"
