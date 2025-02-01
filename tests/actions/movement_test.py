from __future__ import annotations

from typing import TYPE_CHECKING, Final

import pytest

from actions import MoveAction
from components import Position
from constants import Color
from entities import enemies
from exceptions import Impossible, OutOfBounds, PathBlocked
from gameworld import GameWorld
from messagelog import MessageLog

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


@pytest.fixture
def message_log() -> MessageLog:
    return MessageLog()


@pytest.mark.depends(on=["tests/gameworld_test.py", "tests/messagelog_test.py"])
class TestMovement:
    def test_movement_unblocked(
        self, gameworld: GameWorld, message_log: MessageLog
    ) -> None:
        for _ in range(4):
            MoveAction(gameworld.player, (0, -1), gameworld.map, message_log).perform()
        assert gameworld.player.components[Position].xy == (5, 1)

    def test__movement_blocked_wall(
        self, gameworld: GameWorld, message_log: MessageLog
    ) -> None:
        with pytest.raises(Impossible) as exc:
            MoveAction(gameworld.player, (0, -1), gameworld.map, message_log).perform()
        assert exc.type is PathBlocked

    def test_movement_blocked_entity(
        self, gameworld: GameWorld, entity: Entity, message_log: MessageLog
    ) -> None:
        with pytest.raises(Impossible) as exc:
            MoveAction(gameworld.player, (0, 1), gameworld.map, message_log).perform()
        assert exc.type is PathBlocked

    def test_movement_out_of_bounds(
        self, gameworld: GameWorld, message_log: MessageLog
    ) -> None:
        with pytest.raises(Impossible) as exc:
            MoveAction(
                gameworld.player, (-500, 0), gameworld.map, message_log
            ).perform()
        assert exc.type is OutOfBounds

    def test_movement_out_of_bounds_message_color(
        self, gameworld: GameWorld, message_log: MessageLog
    ) -> None:
        with pytest.raises(Impossible):
            MoveAction(
                gameworld.player, (-500, 0), gameworld.map, message_log
            ).perform()
        assert message_log.messages[-1].color == Color.IMPOSSIBLE

    def test_movement_out_of_bounds_message_text(
        self, gameworld: GameWorld, message_log: MessageLog
    ) -> None:
        with pytest.raises(Impossible):
            MoveAction(
                gameworld.player, (-500, 0), gameworld.map, message_log
            ).perform()
        assert message_log.messages[-1].plain_text == "There is nothing but the Void in that direction"