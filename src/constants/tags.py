"""Game tags"""

from __future__ import annotations

from enum import Enum, Flag, auto


class EntityTags(Enum):
    """Entity tags"""

    EQUIPPED = auto()
    HOLDING = auto()
    ITEM = auto()
    HELD_BY = auto()
    HOSTILE = auto()
    FRIENDLY = auto()
    EQUIPPED_BY = auto()


class AIType(Enum):
    """Entity AI types"""

    CONFUSED = auto()
    WANDERING = auto()
    HOSTILE = auto()
    HOWL_RESPONSE = auto()


class ItemType(Flag):
    """Item types"""

    ONE_HAND = auto()
    TWO_HAND = auto()
    MUNDANE = auto()
    MAGICAL = auto()
    SECONDARY = auto()
    SHIELD = auto()
    BODY_ARMOR = auto()
    GAUNTLETS = auto()
    BOOTS = auto()
    GREAVES = auto()
    HELMET = auto()
    CONSUMABLE = auto()
