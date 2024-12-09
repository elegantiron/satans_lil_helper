"""Contains component definitions for use with the ECS framework.

.. todo::
   - [ ] Implement special attacks

"""

from __future__ import annotations

import attrs
from typing import TYPE_CHECKING

from constants import EQUIPMENT

if TYPE_CHECKING:
    from pygame import Surface

@attrs.define
class Renderable:
    """Image to use when drawing this entity to the screen."""
    image: Surface


@attrs.define
class Position:
    x: int
    y: int


@attrs.define
class LastSeen:
    x: int
    y: int


@attrs.define
class Name:
    name: str


@attrs.define
class Health:
    hp: int
    max_hp: int
    per_level: int
    def __init__(self, hp: int, per_level: int = 0):
        self.hp = hp
        self.max_hp: int = hp
        self.per_level = per_level


@attrs.define
class Mana:
    mp: int
    max_mp: int
    per_level: int
    def __init__(self, mp, per_level: int = 0):
        self.mp = mp
        self.max_mp = mp
        self.per_level = per_level


@attrs.define
class Stat:
    base: int
    level: float = 0


@attrs.define
class Strength(Stat):
    pass


@attrs.define
class Magic(Stat):
    pass


@attrs.define
class PhysicalDefense(Stat):
    pass


@attrs.define
class MagicDefense(Stat):
    pass


@attrs.define
class Evade(Stat):
    pass


@attrs.define
class Crit(Stat):
    pass


@attrs.define
class Agility(Stat):
    pass


@attrs.define
class LightRadius(Stat):
    pass


@attrs.define
class NextAction:
    dur: int


@attrs.define
class Equipment:
    usable: dict[EQUIPMENT, bool]
    def __init__(
        self,
        *,
        one_hand: bool = False,
        two_hand: bool = False,
        mundane: bool = False,
        magical: bool = False,
        sec_weap: bool = False,
        shield: bool = False,
        body: bool = False,
        hands: bool = False,
        feet: bool = False,
        legs: bool = False,
        head: bool = False,
    ):
        self.usable = {
            EQUIPMENT.HEAD: head,
            EQUIPMENT.ONE_HAND: one_hand,
            EQUIPMENT.TWO_HAND: two_hand,
            EQUIPMENT.MUNDANE: mundane,
            EQUIPMENT.MAGICAL: magical,
            EQUIPMENT.SECONDARY_WEAPON: sec_weap,
            EQUIPMENT.SHIELD: shield,
            EQUIPMENT.BODY: body,
            EQUIPMENT.HANDS: hands,
            EQUIPMENT.FEET: feet,
            EQUIPMENT.LEGS: legs,
        }

@attrs.define
class Damage:
    dice: int
    sides: int
    slot: int = 0


@attrs.define
class Inventory:
    size: int


@attrs.define
class Level:
    level: int = 1
    xp: int = 0
    xp_granted: int = 0