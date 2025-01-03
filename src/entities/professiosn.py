from __future__ import annotations
from typing import TYPE_CHECKING
from components import Stats, Specials
from constants import SpecialAttacks, ActiveAbilities

if TYPE_CHECKING:
    import tcod.ecs


def warrior_class(entity: tcod.ecs.Entity):
    entity.components |= {
        Stats: Stats(hp=50, mp=5, strength=5, pdef=7, mdef=2, sight=6, light=6),
        Specials: Specials(attacks=[SpecialAttacks.Charge], skills=[ActiveAbilities.ShieldUp])
    }
