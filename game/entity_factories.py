from __future__ import annotations
from typing import TYPE_CHECKING

from game.professions import Profession
from game.components import CoolDown, Inventory, Level, Name, Position, Renderable, Sight
from game.constants import Sprites

if TYPE_CHECKING:
    import tcod.ecs


def make_player(entity: tcod.ecs.Entity, profesion: Profession, position: tuple[int, int]):
    entity.components |= {
        Position: Position(*position),
        CoolDown: CoolDown(0),
        Inventory: Inventory(30),
        Renderable: Renderable(Sprites.Player),
        Name: Name("Player"),
        Sight: Sight(6, 6),
        Level: Level(1, 0, 0),
    }
