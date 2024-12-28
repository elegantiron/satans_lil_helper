from __future__ import annotations

from random import Random
from typing import TYPE_CHECKING

from components import Attack, Stats
from constants import Tags

if TYPE_CHECKING:
    import tcod.ecs


def Wolf(entity: tcod.ecs.Entity, rng: Random):
    entity.components[Stats] = Stats(
        rng.randint(1, 8) + 16,
        strength=2,
        pdef=5,
        crit=1,
        sight=8,
    )
    entity.components[Attack] = Attack(1, 6)
    howl = entity.registry.new_entity()
    gnaw = entity.registry.new_entity()
    entity.relation_tags_many[Tags.SpecialAttacks].add(howl)
    entity.relation_tags_many[Tags.SpecialAttacks].add(gnaw)
