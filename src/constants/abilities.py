"""Ability enums"""

from enum import Enum, auto


class PassiveAbilities(Enum):
    """Passives"""

    PACK_TACTICS = auto()
    DARK_VISION = auto()


class SpecialAttacks(Enum):
    """Special attacks"""

    GNAW = auto()
    CHARGE = auto()


class ActiveAbilities(Enum):
    """Active (non-attack) abilities"""

    HOWL = auto()
    SHIELD_UP = auto()
