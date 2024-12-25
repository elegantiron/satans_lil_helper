from __future__ import annotations

import attrs


@attrs.define
class Stat:
    base: int
    per_level: int


@attrs.define
class Strength(Stat):
    pass


@attrs.define
class MagicPower(Stat):
    pass


@attrs.define
class PhysDef(Stat):
    pass


@attrs.define
class MagicDef(Stat):
    pass


@attrs.define
class Evasion(Stat):
    pass


@attrs.define
class Crit(Stat):
    pass


@attrs.define
class Speed(Stat):
    pass


@attrs.define
class Health:
    hp: int
    max_hp: int

    def __init__(self, hp: int) -> None:
        self.hp = hp
        self.max_hp = hp


@attrs.define
class Mana:
    mp: int
    max_mp: int

    def __init__(self, mp: int) -> None:
        self.mp = mp
        self.max_mp = mp
