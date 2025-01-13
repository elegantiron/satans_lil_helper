"""Entity factories for player professions"""

from __future__ import annotations

from typing import TYPE_CHECKING

from components import (
    ActionDelay,
    Attack,
    Inventory,
    Name,
    Position,
    Skills,
    Stats,
)
from constants import abilities

if TYPE_CHECKING:
    import tcod.ecs


def warrior_class(entity: tcod.ecs.Entity):
    """Give an entity a warrior's stats"""
    entity.components |= {
        Stats: Stats(hp=50, mp=5, strength=5, pdef=7, mdef=2, sight=6, light=6),
        Skills: Skills(
            onetime=abilities.NonRepeatable.ITEM_SLOT_WEAPON
            | abilities.NonRepeatable.ITEM_SLOT_SHIELD
            | abilities.NonRepeatable.ITEM_SLOT_ARMOR
            | abilities.NonRepeatable.ITEM_SLOT_HELM
            | abilities.NonRepeatable.ITEM_SLOT_GAUNTLETS
            | abilities.NonRepeatable.ITEM_SLOT_BOOTS
            | abilities.NonRepeatable.ITEM_TYPE_ONE_HAND
            | abilities.NonRepeatable.ITEM_TYPE_MUNDANE
            | abilities.NonRepeatable.SHIELD_UP
            | abilities.NonRepeatable.CHARGE
        ),
        Position: Position(x=5, y=5, sprite=":images:player/player.png"),
        ActionDelay: ActionDelay(0),
        Inventory: Inventory(26),
        Attack: Attack(1, 8),
        Name: Name("you"),
    }
