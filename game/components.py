from __future__ import annotations
from typing import TYPE_CHECKING

import attrs

if TYPE_CHECKING:
    from constants import Sprites

@attrs.define
class Position:
    x: int
    y: int

    @property
    def xy(self) -> tuple[int, int]:
        return self.x, self.y
    
    def scaled(self, factor:int=32) -> tuple[int, int]:
        return self.x*factor, self.y*factor
    
@attrs.define
class CoolDown:
    dur: int

@attrs.define
class Inventory:
    size: int

@attrs.define
class Renderable:
    sprite: Sprites

@attrs.define
class Name:
    name: str

@attrs.define
class Sight:
    light: int
    vision: int

@attrs.define
class Level:
    level: int
    xp: int
    xp_granted: int

@attrs.define
class Health:
    def __init__(self, hp: int) -> None:
        self.hp = hp
        self.max_hp = hp
        
@attrs.define
class EntityAI:
    type: str