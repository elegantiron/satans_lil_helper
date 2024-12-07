from __future__ import annotations

import attrs
from typing import TYPE_CHECKING

from constants import EQUIPMENT

if TYPE_CHECKING:
    from pygame import Surface


@attrs.define(frozen=True)
class Renderable:
    image: Surface


@attrs.define(frozen=True)
class Position:
    x: int
    y: int


@attrs.define(frozen=True)
class LastSeen:
    x: int
    y: int


@attrs.define(frozen=True)
class Name:
    name: str


@attrs.define(frozen=True)
class Health:
    def __init__(self, hp: int, per_level: int = 0):
        self.hp = hp
        self.max_hp: int = hp
        self.per_level = per_level


@attrs.define(frozen=True)
class Mana:
    def __init__(self, mp, per_level: int = 0):
        self.mp = mp
        self.max_mp = mp
        self.per_level = per_level


@attrs.define(frozen=True)
class Stat:
    base: int
    level: float


@attrs.define(frozen=True)
class Strength(Stat):
    pass


@attrs.define(frozen=True)
class Magic(Stat):
    pass


@attrs.define(frozen=True)
class PhysicalDefense(Stat):
    pass


@attrs.define(frozen=True)
class MagicDefense(Stat):
    pass


@attrs.define(frozen=True)
class Evade(Stat):
    pass


@attrs.define(frozen=True)
class Crit(Stat):
    pass


@attrs.define(frozen=True)
class Agility(Stat):
    pass


@attrs.define(frozen=True)
class LightRadius(Stat):
    pass


@attrs.define(frozen=True)
class NextAction:
    dur: int


@attrs.define(frozen=True)
class Equipment:
    def __init__(
        self,
        *,
        one_hand: bool,
        two_hand: bool,
        mundane: bool,
        magical: bool,
        sec_weap: bool,
        sec_shield: bool,
        body: bool = True,
        hands: bool = True,
        feet: bool = True,
        legs: bool = True,
        head: bool = True,
    ):
        self.usable = {
            EQUIPMENT.HEAD: head,
            EQUIPMENT.ONE_HAND: one_hand,
            EQUIPMENT.TWO_HAND: two_hand,
            EQUIPMENT.MUNDANE: mundane,
            EQUIPMENT.MAGICAL: magical,
            EQUIPMENT.SECONDARY_WEAPON: sec_weap,
            EQUIPMENT.SECONDARY_SHIELD: sec_shield,
            EQUIPMENT.BODY: body,
            EQUIPMENT.HANDS: hands,
            EQUIPMENT.FEET: feet,
            EQUIPMENT.LEGS: legs,
        }


@attrs.define(frozen=True)
class OneHandWeapon:
    dice: int
    sides: int
    slot: int = EQUIPMENT.ONE_HAND | EQUIPMENT.MUNDANE


@attrs.define(frozen=True)
class TwoHandWeapon:
    dice: int
    sides: int
    slot: EQUIPMENT = EQUIPMENT.TWO_HAND | EQUIPMENT.MUNDANE


@attrs.define(frozen=True)
class Staff:
    dice: int
    sides: int
    slot: int = EQUIPMENT.TWO_HAND | EQUIPMENT.MAGICAL


@attrs.define(frozen=True)
class Wand:
    dice: int
    sides: int
    slot: int = EQUIPMENT.WAND | EQUIPMENT.MAGICAL


@attrs.define(frozen=True)
class HeadArmor:
    slot: int = EQUIPMENT.HEAD


@attrs.define(frozen=True)
class BodyArmor:
    slot: int = EQUIPMENT.BODY


@attrs.define(frozen=True)
class Gloves:
    slot: int = EQUIPMENT.HANDS


@attrs.define(frozen=True)
class Boots:
    slot: int = EQUIPMENT.FEET


@attrs.define(frozen=True)
class Pants:
    slot: int = EQUIPMENT.LEGS


@attrs.define(frozen=True)
class Inventory:
    size: int


@attrs.define(frozen=True)
class Level:
    def __init__(self, *, level: int = 0, xp: int = 0, xp_granted: int = 0):
        self.level = level
        self.xp = xp
        self.xp_granted = xp_granted
