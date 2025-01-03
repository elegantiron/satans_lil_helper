from enum import Enum, auto


class PassiveAbilities(Enum):
    PackTactics = auto()
    DarkVision = auto()


class SpecialAttacks(Enum):
    Gnaw = auto()
    Charge = auto()


class ActiveAbilities(Enum):
    Howl = auto()
    ShieldUp = auto()
