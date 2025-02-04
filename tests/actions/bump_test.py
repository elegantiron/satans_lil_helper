# pylint: disable=redefined-outer-name

from __future__ import annotations

from typing import TYPE_CHECKING, Final

import pytest

from actions import BumpAction
from components import Position, Stats
from entities import enemies
from gameworld import GameWorld
from messagelog import MessageLog

if TYPE_CHECKING:
    from collections.abc import Iterator

    from tcod.ecs import Entity

SEED: Final[float] = 1737855529.0953882


@pytest.fixture(scope="class")
def gameworld() -> GameWorld:
    return GameWorld(SEED)


@pytest.fixture
def entity(gameworld: GameWorld) -> Iterator[Entity]:
    nentity = gameworld.spawn_entity(
        enemies.forest.wolf,
        (
            gameworld.player.components[Position].x,
            gameworld.player.components[Position].y - 1,
        ),
    )
    yield nentity
    nentity.clear()


@pytest.fixture
def message_log() -> MessageLog:
    return MessageLog()


@pytest.mark.depends(on=["GameWorld", "MessageLog"])
class TestBump:
    def test_bump_empty_space(
        self, gameworld: GameWorld, message_log: MessageLog
    ) -> None:
        old_position_x = gameworld.player.components[Position].x
        old_position_y = gameworld.player.components[Position].y
        BumpAction(
            gameworld.player, (0, -1), gameworld.map, gameworld.rng, message_log
        ).perform()
        assert gameworld.player.components[Position].xy == (
            old_position_x,
            old_position_y - 1,
        )

    def test_bump_into_enemy_no_movement(
        self, gameworld: GameWorld, message_log: MessageLog, entity: Entity
    ) -> None:
        old_position_x = gameworld.player.components[Position].x
        old_position_y = gameworld.player.components[Position].y
        BumpAction(
            gameworld.player, (0, -1), gameworld.map, gameworld.rng, message_log
        ).perform()
        assert gameworld.player.components[Position].xy == (
            old_position_x,
            old_position_y,
        )

    def test_bump_into_enemy_damage(
        self, gameworld: GameWorld, message_log: MessageLog, entity: Entity
    ) -> None:
        old_hp = entity.components[Stats].hp
        BumpAction(
            gameworld.player, (0, -1), gameworld.map, gameworld.rng, message_log
        ).perform()
        new_hp = entity.components[Stats].hp
        assert new_hp < old_hp
