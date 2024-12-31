from __future__ import annotations

from enum import Enum, auto


class Tags(Enum):
    SpecialAttacks = auto()
    Equipped = auto()
    Holding = auto()
    Item = auto()
    HeldBy = auto()
    Hostile = auto()
    Friendly = auto()