from enum import StrEnum

from . import keylists
from .sectionnames import Sections
from .strings import Strings
from . import colors

__all__ = ["Strings", "Sections", "keylists", "colors"]

TILE_SIZE = 32


class Tile(StrEnum):
    Walkable = "walkable"
    Transparent = "transparent"
    Explored = "explored"
    Visible = "visible"
    Safe = "safe"
    MovementCost = "movement cost"
    SpritePath = "sprite path"
