from __future__ import annotations

from typing import TYPE_CHECKING

import attrs

from constants.abilities import SpecialAttacks

from .ailments import Confusion
from .position import Position
from .stats import Growth, Stats
from .equipment import Equipment

if TYPE_CHECKING:
    from constants import ActiveAbilities, AIType, PassiveAbilities

__all__ = [
    "Position",
    "Growth",
    "Stats",
    "Attack",
    "Inventory",
    "Name",
    "ActionDelay",
    "Specials",
    "AI",
    "Confusion",
    "Equipment",
]


@attrs.define
class Attack:
    dice: int
    sides: int


@attrs.define
class Inventory:
    size: int = 0


@attrs.define
class Name:
    name: str


@attrs.define
class ActionDelay:
    ticks: int = 0


@attrs.define(kw_only=True)
class Specials:
    attacks: list[SpecialAttacks] = []
    passives: list[PassiveAbilities] = []
    skills: list[ActiveAbilities] = []


@attrs.define
class AI:
    type: AIType
    base_type: AIType
    path: list[tuple[int, int]] = []
