"""Components for entities"""

from __future__ import annotations

from typing import TYPE_CHECKING

import attrs

from .ailments import Confusion
from .items import Equippable
from .position import Position
from .stats import Growth, Stats

if TYPE_CHECKING:
    from constants import AIType, abilities

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

    repeatable: dict[abilities.Repeatable, int] = dict()
    onetime: abilities.Abilities = 0


@attrs.define
class AI:
    """An entity's brains"""

    type: AIType
    base_type: AIType
    path: list[tuple[int, int]] = []
