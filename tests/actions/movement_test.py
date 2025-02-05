from __future__ import annotations

from typing import TYPE_CHECKING

import pytest

from actions import MoveAction
from components import Position
from constants import Color
from exceptions import Impossible, OutOfBounds, PathBlocked

if TYPE_CHECKING:
    from tcod.ecs import Entity

    from gameworld import GameWorld
    from messagelog import MessageLog


@pytest.mark.depends(on=["GameWorld", "MessageLog"])
class TestMovement:
    def test_movement_unblocked(
        self, gameworld_fixed_seed: GameWorld, message_log: MessageLog
    ) -> None:
        for _ in range(4):
            MoveAction(
                gameworld_fixed_seed.player,
                (0, -1),
                gameworld_fixed_seed.map,
                message_log,
            ).perform()
        assert gameworld_fixed_seed.player.components[Position].xy == (5, 1)

    def test__movement_blocked_wall(
        self, gameworld_fixed_seed: GameWorld, message_log: MessageLog
    ) -> None:
        with pytest.raises(Impossible) as exc:
            MoveAction(
                gameworld_fixed_seed.player,
                (0, -1),
                gameworld_fixed_seed.map,
                message_log,
            ).perform()
        assert exc.type is PathBlocked

    def test_movement_blocked_entity(
        self, gameworld_fixed_seed: GameWorld, wolf_one_above: Entity, message_log: MessageLog
    ) -> None:
        with pytest.raises(Impossible) as exc:
            MoveAction(
                gameworld_fixed_seed.player,
                (0, 1),
                gameworld_fixed_seed.map,
                message_log,
            ).perform()
        assert exc.type is PathBlocked

    def test_movement_out_of_bounds(
        self, gameworld_fixed_seed: GameWorld, message_log: MessageLog
    ) -> None:
        with pytest.raises(Impossible) as exc:
            MoveAction(
                gameworld_fixed_seed.player,
                (-500, 0),
                gameworld_fixed_seed.map,
                message_log,
            ).perform()
        assert exc.type is OutOfBounds

    def test_movement_out_of_bounds_message_color(
        self, gameworld_fixed_seed: GameWorld, message_log: MessageLog
    ) -> None:
        with pytest.raises(Impossible):
            MoveAction(
                gameworld_fixed_seed.player,
                (-500, 0),
                gameworld_fixed_seed.map,
                message_log,
            ).perform()
        assert message_log.messages[-1].color == Color.IMPOSSIBLE

    def test_movement_out_of_bounds_message_text(
        self, gameworld_fixed_seed: GameWorld, message_log: MessageLog
    ) -> None:
        with pytest.raises(Impossible):
            MoveAction(
                gameworld_fixed_seed.player,
                (-500, 0),
                gameworld_fixed_seed.map,
                message_log,
            ).perform()
        assert (
            message_log.messages[-1].plain_text
            == "There is nothing but the Void in that direction"
        )
