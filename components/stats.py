from __future__ import annotations

import attrs


class Stats:
    def __init__(
        self,
        hp: float = 0,
        mp: float = 0,
        strength: float = 0,
        magic: float = 0,
        pdef: float = 0,
        mdef: float = 0,
        evasion: float = 0,
        crit: float = 0,
        sight: float = 0,
        light: float = 0,
    ):
        self.hp = hp
        self.mp = mp
        self.strength = strength
        self.magic = magic
        self.pdef = pdef
        self.mdef = mdef
        self.evasion = evasion
        self.crit = crit
        self.sight = sight
        self.light = light

    @property
    def hp(self) -> float:
        return self._hp

    @hp.setter
    def hp(self, value) -> None:
        if not getattr(self, "max_hp", None):
            self.max_hp = value
        self._hp = max(0, min(value, self.max_hp))

    @property
    def mp(self) -> float:
        return self._mp

    @mp.setter
    def mp(self, value) -> None:
        if not getattr(self, "max_mp", None):
            self.max_mp = value
        self._mp = max(0, min(value, self.max_mp))


@attrs.define
class Growth:
    hp: float = 0
    mp: float = 0
    strength: float = 0
    magic: float = 0
    pdef: float = 0
    mdef: float = 0
    evasion: float = 0
    crit: float = 0
    sight: float = 0
    light: float = 0
