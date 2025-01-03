from enum import StrEnum

from . import colors, keylists
from .abilities import ActiveAbilities, PassiveAbilities, SpecialAttacks
from .sectionnames import Sections
from .strings import Strings
from .tags import AIType, EntityTags, ItemType

__all__ = [
    "Strings",
    "Sections",
    "keylists",
    "colors",
    "EntityTags",
    "PassiveAbilities",
    "SpecialAttacks",
    "ActiveAbilities",
    "AIType",
    "ItemType",
]

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
