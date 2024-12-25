from __future__ import annotations

from typing import TYPE_CHECKING

from ..components import EntityAI, Health, Name, Position
from ..constants import EnemyType

if TYPE_CHECKING:
    from random import Random

    import tcod.ecs


def make_wolf(entity: tcod.ecs.Entity, position: tuple[int, int], rng: Random) -> None:
    entity.components |= {
        Position: Position(*position),
        Name: Name(EnemyType.Wolf),
        Health: Health(rng.randint(1, 8) + 16),
        EntityAI: EntityAI(EnemyType.Wolf),
    }
