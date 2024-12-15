from __future__ import annotations
from typing import TYPE_CHECKING

from game.components import CoolDown, Inventory, Level, Name, Position, Renderable, Sight
from game.constants import Sprites, Professions

if TYPE_CHECKING:
    import tcod.ecs


def make_player(entity: tcod.ecs.Entity, profesion: Professions, position: tuple[int, int]):
    entity.components |= {
        Position: Position(*position),
        CoolDown: CoolDown(0),
        Inventory: Inventory(30),
        Renderable: Renderable(Sprites.PLAYER),
        Name: Name("Player"),
        Sight: Sight(6, 6),
        Level: Level(1, 0, 0),
    }
