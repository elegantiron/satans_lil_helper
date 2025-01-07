"""Components for entities"""

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
    """An entity's attack damage"""
    dice: int
    sides: int


@attrs.define
class Inventory:
    """How many items an entity can hold"""
    size: int = 0


@attrs.define
class Name:
    """An entity's name"""
    name: str


@attrs.define
class ActionDelay:
    """Tracks how long until an entity's next action"""
    ticks: int = 0


@attrs.define(kw_only=True)
class Specials:
    """An entity's special abilities"""
    attacks: SpecialAttacks = 0
    passives: PassiveAbilities = 0
    skills: ActiveAbilities = 0


@attrs.define
class AI:
    """An entity's brains"""
    type: AIType
    base_type: AIType
    path: list[tuple[int, int]] = []
