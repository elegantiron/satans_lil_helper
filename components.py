from __future__ import annotations

from dataclasses import dataclass as component
from dataclasses import field
from typing import TYPE_CHECKING

from constants import EQUIPMENT

if TYPE_CHECKING:
    from pygame import Surface


@component
class Renderable:
    image: Surface


@component
class Position:
    x: int
    y: int


@component
class Health:
    def __init__(self, hp):
        self._hp = hp
        self.max_hp = hp

    @property
    def hp(self) -> int:
        return self._hp

    @hp.setter
    def hp(self, value: int) -> None:
        self._hp = max(0, min(value, self.max_hp))


@component
class Mana:
    def __init__(self, mp):
        self._mp = mp
        self.max_mp = mp

    @property
    def mp(self) -> int:
        return self._mp

    @mp.setter
    def mp(self, value: int) -> None:
        self._mp = max(0, min(value, self.max_mp))


@component
class Stats:
    str: int = 0
    mag: int = 0
    pdef: int = 0
    mdef: int = 0
    eva: int = 0
    crit: int = 0
    agi: int = 0
    fov: int = 0


@component
class NextAction:
    dur: int


@component
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


@component
class Equipped:
    pass

@component
class IsItem:
    pass

@component
class OneHandWeapon:
    dice: int
    sides: int
    slot: int = field(init=False, default=EQUIPMENT.ONE_HAND | EQUIPMENT.MUNDANE)


@component
class TwoHandWeapon:
    dice: int
    sides: int
    slot: EQUIPMENT = field(init=False, default=EQUIPMENT.TWO_HAND | EQUIPMENT.MUNDANE)


@component
class Staff:
    dice: int
    sides: int
    slot: int = field(init=False, default=EQUIPMENT.TWO_HAND | EQUIPMENT.MAGICAL)


@component
class Wand:
    dice: int
    sides: int
    slot: int = field(init=False, default=EQUIPMENT.WAND | EQUIPMENT.MAGICAL)


@component
class HeadArmor:
    slot: int = field(init=False, default=EQUIPMENT.HEAD)


@component
class BodyArmor:
    slot: int = field(init=False, default=EQUIPMENT.BODY)


@component
class Gloves:
    slot: int = field(init=False, default=EQUIPMENT.HANDS)


@component
class Boots:
    slot: int = field(init=False, default=EQUIPMENT.FEET)


@component
class Pants:
    slot: int = field(init=False, default=EQUIPMENT.LEGS)
