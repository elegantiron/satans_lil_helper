from __future__ import annotations

from typing import TYPE_CHECKING

from components import Position

if TYPE_CHECKING:
    import random

    import tcod.ecs


def health_potion(
    entity: tcod.ecs.Entity, position: tuple[int, int], rng: random.Random
) -> None:
    entity.components |= {
        Position: Position(x=position[0], y=position[1]),
    }  # pyright: ignore[reportOperatorIssue]
