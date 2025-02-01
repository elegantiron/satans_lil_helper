"""Components for entities"""

from __future__ import annotations

from typing import TYPE_CHECKING

import attrs

from .ailments import Confusion, DamagingAilment
from .items import Equippable
from .position import Position
from .stats import Growth, Stats

if TYPE_CHECKING:
    import abilities
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
    "DamagingAilment",
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
    definite_article: str = attrs.field(default=None, kw_only=True)
    indefinite_article: str = attrs.field(default=None, kw_only=True)


@attrs.define
class ActionDelay:
    """Tracks how long until an entity's next action"""

    ticks: int = 0


@attrs.define(kw_only=True, init=False)
class Skills:
    """An entity's special abilities"""

    repeatable: dict[abilities.Abilities, int]
    onetime: abilities.Abilities

    def __init__(
        self,
        *,
        onetime: abilities.Abilities = 0,  # type: ignore
        repeatable: dict[abilities.Abilities, int] | None = None,
    ) -> None:
        import abilities  # pylint: disable=import-outside-toplevel

        self.onetime = onetime
        if repeatable is not None:
            self.repeatable = repeatable
        else:
            self.repeatable = {
                skill.skill_id: 0
                for skill in abilities.SkillList
                if skill.onetime is False
            }


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
