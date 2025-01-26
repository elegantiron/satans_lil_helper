"""Entity factories for player professions"""

from __future__ import annotations

from typing import TYPE_CHECKING

import abilities
from components import (
    ActionDelay,
    Attack,
    Inventory,
    Name,
    Position,
    Skills,
    Stats,
)

if TYPE_CHECKING:
    from random import Random

    import tcod.ecs


def warrior_class(
    entity: tcod.ecs.Entity, position: tuple[int, int], rng: Random | None = None
) -> None:
    """Give an entity a warrior's stats"""
    entity.components |= {
        Stats: Stats(hp=50, mp=5, strength=5, pdef=7, mdef=2, sight=6, light=6),
        Skills: Skills(
            onetime=abilities.Abilities.ITEM_SLOT_WEAPON
            | abilities.Abilities.ITEM_SLOT_SHIELD
            | abilities.Abilities.ITEM_SLOT_ARMOR
            | abilities.Abilities.ITEM_SLOT_HELM
            | abilities.Abilities.ITEM_SLOT_GAUNTLETS
            | abilities.Abilities.ITEM_SLOT_BOOTS
            | abilities.Abilities.ITEM_SLOT_GREAVES
            | abilities.Abilities.ITEM_TYPE_ONE_HAND
            | abilities.Abilities.ITEM_TYPE_MUNDANE
            | abilities.Abilities.SHIELD_UP
            | abilities.Abilities.CHARGE
        ),
        Position: Position(
            x=position[0], y=position[1], sprite=":images:player/player.png"
        ),
        ActionDelay: ActionDelay(0),
        Inventory: Inventory(26),
        Attack: Attack(1, 8),
        Name: Name("you"),
    }
