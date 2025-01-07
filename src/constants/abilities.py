"""Ability enums"""

from enum import Enum, Flag, auto


class Enemies(Flag):
    """Enemy abilities"""

    PACK_TACTICS = auto()
    DARK_VISION = auto()
    GNAW = auto()
    HOWL = auto()


class General(Flag):
    ELEMENTAL_AFFINITY_I = auto()
    ELEMENTAL_AFFINITY_II = auto()
    ELEMENTAL_AFFINITY_III = auto()

    LIGHT_UP = auto()

    STATUS_MASTER_I = auto()
    STATUS_MASTER_II = auto()
    STATUS_MASTER_III = auto()

    ITEM_SLOT_SHIELD = auto()
    ITEM_SLOT_ARMOR = auto()
    ITEM_SLOT_HELM = auto()
    ITEM_SLOT_GAUNTLETS = auto()
    ITEM_SLOT_GREAVES = auto()
    ITEM_SLOT_BOOTS = auto()


class Repeatable(Enum):
    STRENGTH_UP = auto()
    MAGIC_UP = auto()
    PDEF_UP = auto()
    MDEF_UP = auto()


class Warrior(Flag):
    SHIELD_UP = auto()
    CHARGE = auto()
