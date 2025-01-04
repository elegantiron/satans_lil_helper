"""Definitions for negative status effects"""

from __future__ import annotations

import attrs


@attrs.define
class Ailment:
    """Base ailment class"""

    turns: int
    limit: int


@attrs.define
class Confusion(Ailment):
    """Confusion effect"""
