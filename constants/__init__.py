from enum import StrEnum

from . import keylists
from .sectionnames import Sections
from .strings import Strings
from . import colors
from .tags import Tags

__all__ = ["Strings", "Sections", "keylists", "colors", "Tags"]

TILE_SIZE = 32

LINE_SPACING = 2


class Tile(StrEnum):
    Walkable = "walkable"
    Transparent = "transparent"
    Explored = "explored"
    Visible = "visible"
    Safe = "safe"
    MovementCost = "movement cost"
    SpritePath = "sprite path"
