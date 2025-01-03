from __future__ import annotations

from enum import Enum, auto


class Tags(Enum):
    Equipped = auto()
    Holding = auto()
    Item = auto()
    HeldBy = auto()
    Hostile = auto()
    Friendly = auto()

class AIType(Enum):
    Confused = auto()
    Wandering = auto()
    Hostile = auto()
    HowlResponse = auto()

class ItemType(Enum):
    OneHand = auto()
    TwoHand = auto()
    Mundane = auto()
    Magical = auto()
    SecondaryWeapon = auto()
    Shield = auto()
    BodyArmor = auto()
    Gauntlets = auto()
    Boots = auto()
    Greaves = auto()
    Helmet = auto()
    Consumable = auto()