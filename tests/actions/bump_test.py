# pylint: disable=redefined-outer-name

from __future__ import annotations

from typing import TYPE_CHECKING

import pytest

from actions import BumpAction
from components import Position, Stats

if TYPE_CHECKING:
    from tcod.ecs import Entity

    from gameworld import GameWorld
    from messagelog import MessageLog


@pytest.mark.depends(on=["GameWorld", "MessageLog"])
class TestBump:
    def test_bump_empty_space(
        self, gameworld_fixed_seed: GameWorld, message_log: MessageLog
    ) -> None:
        old_position_x = gameworld_fixed_seed.player.components[Position].x
        old_position_y = gameworld_fixed_seed.player.components[Position].y
        BumpAction(
            gameworld_fixed_seed.player,
            (0, -1),
            gameworld_fixed_seed.map,
            gameworld_fixed_seed.rng,
            message_log,
        ).perform()
        assert gameworld_fixed_seed.player.components[Position].xy == (
            old_position_x,
            old_position_y - 1,
        )

    def test_bump_into_enemy_no_movement(
        self,
        gameworld_fixed_seed: GameWorld,
        message_log: MessageLog,
        wolf_one_below: Entity,
    ) -> None:
        old_position_x = gameworld_fixed_seed.player.components[Position].x
        old_position_y = gameworld_fixed_seed.player.components[Position].y
        BumpAction(
            gameworld_fixed_seed.player,
            (0, -1),
            gameworld_fixed_seed.map,
            gameworld_fixed_seed.rng,
            message_log,
        ).perform()
        assert gameworld_fixed_seed.player.components[Position].xy == (
            old_position_x,
            old_position_y,
        )

    def test_bump_into_enemy_damage(
        self,
        gameworld_fixed_seed: GameWorld,
        message_log: MessageLog,
        wolf_one_below: Entity,
    ) -> None:
        old_hp = wolf_one_below.components[Stats].hp
        BumpAction(
            gameworld_fixed_seed.player,
            (0, -1),
            gameworld_fixed_seed.map,
            gameworld_fixed_seed.rng,
            message_log,
        ).perform()
        new_hp = wolf_one_below.components[Stats].hp
        assert new_hp < old_hp
