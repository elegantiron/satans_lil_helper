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