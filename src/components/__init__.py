"""Components for entities"""

from __future__ import annotations

from typing import TYPE_CHECKING

import attrs

from constants import abilities

from .ailments import Confusion
from .items import Equippable
from .position import Position
from .stats import Growth, Stats

if TYPE_CHECKING:
    from constants import AIType

__all__ = [
    "Position",
    "Growth",
    "Stats",
    "Attack",
    "Inventory",
    "Name",
    "ActionDelay",
    "Skills",
    "AI",
    "Confusion",
    "Equippable",
    "Regen",
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
class Skills:
    """An entity's special abilities"""

    repeatable: dict[abilities.Abilities, int]
    onetime: abilities.Abilities

    def __init__(self, *, onetime: abilities.Abilities = 0) -> None:
        self.repeatable = {
            skill.skill_id: 0 for skill in abilities.SkillList if not skill.onetime
        }
        self.onetime = onetime


@attrs.define
class AI:
    """An entity's brains"""

    type: AIType
    base_type: AIType
    path: list[tuple[int, int]] = []


@attrs.define
class Regen:
    """An entity's health and mana regeneration"""

    health: float
    mana: float
    interval: int
    counter: int

    @property
    def proc(self) -> bool:
        return (self.counter % self.interval) == 0
