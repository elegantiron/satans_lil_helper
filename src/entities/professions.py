"""Entity factories for player professions"""

from __future__ import annotations

from typing import TYPE_CHECKING

from components import (
    ActionDelay,
    Attack,
    Equipment,
    Inventory,
    Name,
    Position,
    Specials,
    Stats,
)
from constants import ActiveAbilities, SpecialAttacks

if TYPE_CHECKING:
    import tcod.ecs


def warrior_class(entity: tcod.ecs.Entity):
    """Give an entity a warrior's stats"""
    entity.components |= {
        Stats: Stats(hp=50, mp=5, strength=5, pdef=7, mdef=2, sight=6, light=6),
        Specials: Specials(
            attacks=SpecialAttacks.CHARGE, skills=ActiveAbilities.SHIELD_UP
        ),
        Equipment: Equipment(
            one_hand=True,
            shield=True,
            body_armor=True,
            helmet=True,
            gauntlets=True,
            boots=True,
            mundane=True,
        ),
        Position: Position(x=5, y=5, sprite=":images:player/player.png"),
        ActionDelay: ActionDelay(0),
        Inventory: Inventory(26),
        Attack: Attack(1, 8),
        Name: Name("you"),
    }
