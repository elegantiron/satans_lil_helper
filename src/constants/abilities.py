"""Ability enums"""

from enum import Flag, auto


class PassiveAbilities(Flag):
    """Passives"""

    PACK_TACTICS = auto()
    DARK_VISION = auto()


class SpecialAttacks(Flag):
    """Special attacks"""

    GNAW = auto()
    CHARGE = auto()


class ActiveAbilities(Flag):
    """Active (non-attack) abilities"""

    HOWL = auto()
    SHIELD_UP = auto()
