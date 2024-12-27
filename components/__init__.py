from __future__ import annotations

import attrs

from .position import Position
from .stats import Growth, Stats

__all__ = ["Position", "Growth", "Stats", "Attack"]


@attrs.define
class Attack:
    dice: int
    sides: int
