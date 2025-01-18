"""Definitions for negative status effects"""

from __future__ import annotations

import attrs


@attrs.define
class Ailment:
    """Base ailment class"""

    age: int
    limit: int
    end_chance_per_turn: float


@attrs.define
class Confusion(Ailment):
    """Confusion effect"""
