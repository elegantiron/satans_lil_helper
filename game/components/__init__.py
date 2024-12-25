from __future__ import annotations

from typing import TYPE_CHECKING

import attrs

from .equipment import Equipment, Equippable
from .stats import (
    Crit,
    Evasion,
    Health,
    MagicDef,
    MagicPower,
    Mana,
    PhysDef,
    Speed,
    Strength,
)

__all__ = [
    "Crit",
    "Equippable",
    "Equipment",
    "Health",
    "Mana",
    "Evasion",
    "MagicDef",
    "MagicPower",
    "PhysDef",
    "Speed",
    "Strength",
    "Position",
    "CoolDown",
    "Inventory",
    "Renderable",
    "Name",
    "Sight",
    "Level",
    "Health",
    "EntityAI",
    "Mana",
]

if TYPE_CHECKING:
    from constants import Sprites


@attrs.define
class Position:
    x: int
    y: int

    @property
    def xy(self) -> tuple[int, int]:
        return self.x, self.y

    def scaled(self, factor: int = 32) -> tuple[int, int]:
        return self.x * factor, self.y * factor


@attrs.define
class CoolDown:
    dur: int


@attrs.define
class Inventory:
    size: int


@attrs.define
class Renderable:
    sprite: Sprites


@attrs.define
class Name:
    name: str


@attrs.define
class Sight:
    light: int
    vision: int


@attrs.define
class Level:
    level: int
    xp: int
    xp_granted: int


@attrs.define
class EntityAI:
    type: str
