from __future__ import annotations

import random
from typing import TYPE_CHECKING

import pytest

from components import ActionDelay, Position
from constants import Tile
from entities import enemies, professions
from exceptions import Impossible, SpawnBlocked

if TYPE_CHECKING:
    from collections.abc import Callable
    from random import Random

    import tcod.ecs

    from gameworld import GameWorld


@pytest.mark.slow
@pytest.mark.depends(on="MessageLog", name="GameWorld")
class TestGameWorld:
    @pytest.mark.parametrize("iteration", range(5))
    def test_player_position(
        self, gameworld_random_seed: GameWorld, iteration: int
    ) -> None:  # pylint: disable=unused-argument
        p_pos = gameworld_random_seed.player.components[Position]
        assert p_pos is not None
        assert gameworld_random_seed.map.tiles[Tile.WALKABLE][p_pos.xy]

    def test_equality(
        self, gameworld_random_seed: GameWorld, gameworld_random_seed2: GameWorld
    ) -> None:
        assert gameworld_random_seed != gameworld_random_seed2
        assert gameworld_random_seed2 == gameworld_random_seed2
        assert gameworld_random_seed == gameworld_random_seed

    @pytest.mark.parametrize("spawn_function", [enemies.forest.wolf])
    def test_entity_spawn(
        self,
        gameworld_random_seed: GameWorld,
        spawn_function: Callable[[tcod.ecs.Entity, tuple[int, int], Random], None],
    ) -> None:
        for _ in range(random.randint(4, 100)):
            gameworld_random_seed.spawn_entity(spawn_function)
        chosen = False
        x: int = 0
        y: int = 0
        while not chosen:
            x = gameworld_random_seed.rng.randint(
                0, len(gameworld_random_seed.map.tiles) - 1
            )
            y = gameworld_random_seed.rng.randint(
                0, len(gameworld_random_seed.map.tiles[x]) - 1
            )
            if gameworld_random_seed.map.tiles[Tile.WALKABLE][x, y]:
                chosen = True
        gameworld_random_seed.spawn_entity(spawn_function, (x, y))
        with pytest.raises(Impossible) as exc:
            gameworld_random_seed.spawn_entity(spawn_function, (0, 0))
        assert exc.type is SpawnBlocked

    def test_time_step(self, gameworld_random_seed: GameWorld) -> None:
        entity = gameworld_random_seed.spawn_entity(professions.warrior_class)
        action_delay = entity.components.get(ActionDelay)
        assert action_delay is not None
        previous_delay = action_delay.ticks
        gameworld_random_seed.step_time()
        assert action_delay.ticks == previous_delay - 1
