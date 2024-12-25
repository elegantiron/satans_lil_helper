from __future__ import annotations

from typing import TYPE_CHECKING

from ..components import CoolDown, Inventory, Level, Name, Position, Renderable, Sight
from ..constants import EquipmentSlot, Profession, Sprites

if TYPE_CHECKING:
    import tcod.ecs


def make_player(
    entity: tcod.ecs.Entity, profession: Profession, position: tuple[int, int]
):
    entity.components |= {
        Position: Position(*position),
        CoolDown: CoolDown(0),
        Inventory: Inventory(30),
        Renderable: Renderable(Sprites.Player),
        Name: Name("Player"),
        Sight: Sight(6, 6),
        Level: Level(1, 0, 0),
    }
    match profession:
        case Profession.Warrior:
            make_warrior(entity)


def make_warrior(entity: tcod.ecs.Entity):
    entity.components |= {
        EquipmentSlot.Head: True,
        EquipmentSlot.Body: True,
        EquipmentSlot.Feet: True,
        EquipmentSlot.Hands: True,
        EquipmentSlot.Weapon: True,
        EquipmentSlot.Shield: True,
        EquipmentSlot.Mundane: True,
        EquipmentSlot.Magic: False,
    }

