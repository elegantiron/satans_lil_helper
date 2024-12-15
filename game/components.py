from __future__ import annotations
from dataclasses import dataclass as component
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from constants import Sprites

@component
class Position:
    x: int
    y: int

    @property
    def xy(self) -> tuple[int, int]:
        return self.x, self.y
    
    def scaled(self, factor:int=32) -> tuple[int, int]:
        return self.x*factor, self.y*factor
    
@component
class CoolDown:
    dur: int

@component
class Inventory:
    size: int

@component
class Renderable:
    sprite: Sprites

@component
class Name:
    name: str

@component
class Sight:
    light: int
    vision: int

@component
class Level:
    level: int
    xp: int
    xp_granted: int

@component
class Health:
    def __init__(self, hp: int) -> None:
        self.hp = hp
        self.max_hp = hp