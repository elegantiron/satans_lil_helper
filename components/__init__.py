from __future__ import annotations

import attrs

from .position import Position
from .stats import Growth, Stats

__all__ = ["Position", "Growth", "Stats", "Attack", "Inventory", "Name"]


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
