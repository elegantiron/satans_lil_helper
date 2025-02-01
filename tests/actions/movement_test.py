from __future__ import annotations

from typing import TYPE_CHECKING, Final

import pytest

from actions import MoveAction
from components import Position
from entities import enemies
from exceptions import Impossible, OutOfBounds, PathBlocked
from gameworld import GameWorld

if TYPE_CHECKING:
    from tcod.ecs import Entity

SEED: Final[float] = 1737855529.0953882

# pylint: disable=redefined-outer-name


@pytest.fixture(scope="class")
def gameworld() -> GameWorld:
    return GameWorld(SEED)


@pytest.fixture(scope="class")
def entity(gameworld: GameWorld) -> Entity:
    return gameworld.spawn_entity(
        enemies.forest.wolf,
        (
            gameworld.player.components[Position].x,
            gameworld.player.components[Position].y + 1,
        ),
    )


class TestMovement:
    def test_movement_unblocked(self, gameworld: GameWorld) -> None:
        for _ in range(4):
            MoveAction(gameworld.player, (0, -1), gameworld.map).perform()
        assert gameworld.player.components[Position].xy == (5, 1)

    def test__movement_blocked_wall(self, gameworld: GameWorld) -> None:
        with pytest.raises(Impossible) as exc:
            MoveAction(gameworld.player, (0, -1), gameworld.map).perform()
        assert exc.type is PathBlocked

    def test_movement_blocked_entity(
        self, gameworld: GameWorld, entity: Entity
    ) -> None:
        with pytest.raises(Impossible) as exc:
            MoveAction(gameworld.player, (0, 1), gameworld.map).perform()
        assert exc.type is PathBlocked

    def test_movement_out_of_bounds(self, gameworld: GameWorld) -> None:
        with pytest.raises(Impossible) as exc:
            MoveAction(gameworld.player, (-500, 0), gameworld.map).perform()
        assert exc.type is OutOfBounds
