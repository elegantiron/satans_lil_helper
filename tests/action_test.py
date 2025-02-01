from __future__ import annotations

from typing import TYPE_CHECKING, Final

import pytest

from actions import BumpAction, MeleeAction, MoveAction
from components import Position, Stats
from entities import enemies
from exceptions import Impossible, OutOfBounds, PathBlocked
from gameworld import GameWorld
from messagelog import MessageLog

if TYPE_CHECKING:
    from tcod.ecs.entity import Entity

SEED: Final = 1737855529.0953882


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


@pytest.fixture(scope="class")
def message_log() -> MessageLog:
    return MessageLog()


class TestActions:
    def test_movement(self, gameworld: GameWorld) -> None:
        for _ in range(4):
            MoveAction(gameworld.player, (0, -1), gameworld.map).perform()
        with pytest.raises(Impossible) as exc:
            MoveAction(gameworld.player, (0, -1), gameworld.map).perform()
        assert exc.type is PathBlocked
        MoveAction(gameworld.player, (-1, 0), gameworld.map).perform()
        with pytest.raises(Impossible) as exc:
            MoveAction(gameworld.player, (-1, 0), gameworld.map).perform()
        assert exc.type is PathBlocked

    def test_out_of_bounds(self, gameworld: GameWorld) -> None:
        with pytest.raises(OutOfBounds) as exc:
            MoveAction(gameworld.player, (-500, 0), gameworld.map).perform()
        assert exc.type is OutOfBounds

    def test_blocked_movement(self, gameworld: GameWorld, entity: Entity) -> None:
        assert entity is not None
        with pytest.raises(PathBlocked) as exc:
            MoveAction(gameworld.player, (0, 1), gameworld.map).perform()
        assert exc.type is PathBlocked

    @pytest.mark.depends(on=["tests/messagelog_test.py::TestMessageLog::test_logging"])
    def test_melee_action(
        self, gameworld: GameWorld, entity: Entity, message_log: MessageLog
    ) -> None:
        assert entity is not None
        old_hp = entity.components[Stats].hp
        MeleeAction(
            gameworld.player, (0, 1), gameworld.map, gameworld.rng, message_log
        ).perform()
        assert old_hp > entity.components[Stats].hp

    @pytest.mark.depends(
        on=["test_movement", "test_blocked_movement", "test_melee_action"]
    )
    def test_bump_action(
        self, gameworld: GameWorld, entity: Entity, message_log: MessageLog
    ) -> None:
        old_hp = entity.components[Stats].hp
        BumpAction(gameworld.player, (0, 1), gameworld.map, gameworld.rng, message_log).perform()
        assert old_hp > entity.components[Stats].hp
        BumpAction(gameworld.player, (1, 0), gameworld.map, gameworld.rng, message_log).perform()