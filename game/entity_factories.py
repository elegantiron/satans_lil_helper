from __future__ import annotations

from typing import TYPE_CHECKING

import tcod.ecs.entity

from game.components import (
    CoolDown,
    EntityAI,
    Health,
    Inventory,
    Level,
    Name,
    Position,
    Renderable,
    Sight,
)
from game.constants import EnemyType, Sprites


if TYPE_CHECKING:
    from random import Random
    from game.professions import Profession
    import tcod.ecs


def make_player(
    entity: tcod.ecs.Entity, profesion: Profession, position: tuple[int, int]
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


def make_wolf(entity: tcod.ecs.Entity, position: tuple[int, int], rng: Random) -> None:
    entity.components |= {
        Position: Position(*position),
        Name: Name(EnemyType.Wolf),
        Health: Health(rng.randint(1, 8) + 16),
        EntityAI: EntityAI(EnemyType.Wolf),
    }
