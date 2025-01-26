from __future__ import annotations

import random
from typing import TYPE_CHECKING

import pytest

from actions import MoveAction
from components import Position
from constants import Tile
from entities import enemies
from exceptions import Impossible, PathBlocked, SpawnBlocked
from gameworld import GameWorld

if TYPE_CHECKING:
    from collections.abc import Callable
    from random import Random

    import tcod.ecs

SEED = 1737855529.0953882


@pytest.fixture
def gameworld() -> GameWorld:
    return GameWorld()


@pytest.fixture
def gameworld2() -> GameWorld:
    return GameWorld(SEED)


@pytest.fixture
def gameworld3() -> GameWorld:
    return GameWorld(SEED)


class TestGameWorld:
    @pytest.mark.parametrize("iteration", range(5))
    def test_player_position(self, gameworld: GameWorld, iteration: int) -> None:
        p_pos = gameworld.player.components[Position]
        assert p_pos is not None
        assert gameworld.map.tiles[Tile.WALKABLE][p_pos.xy]

    def test_equality(self, gameworld: GameWorld, gameworld2: GameWorld) -> None:
        assert gameworld != gameworld2
        assert gameworld2 == gameworld2
        assert gameworld == gameworld

    def test_movement(self, gameworld2: GameWorld) -> None:
        for _ in range(4):
            MoveAction(gameworld2.player, (0, -1), gameworld2.map).perform()
        with pytest.raises(Impossible) as exc:
            MoveAction(gameworld2.player, (0, -1), gameworld2.map).perform()
        assert exc.type is PathBlocked
        MoveAction(gameworld2.player, (-1, 0), gameworld2.map).perform()
        with pytest.raises(Impossible) as exc:
            MoveAction(gameworld2.player, (-1, 0), gameworld2.map).perform()
        assert exc.type is PathBlocked

    @pytest.mark.parametrize("spawn_function", [enemies.forest.wolf])
    def test_entity_spawn(
        self,
        gameworld: GameWorld,
        spawn_function: Callable[[tcod.ecs.Entity, tuple[int, int], Random], None],
    ) -> None:
        for _ in range(random.randint(4, 100)):
            gameworld.spawn_entity(spawn_function)
        chosen = False
        while not chosen:
            x = gameworld.rng.randint(0, len(gameworld.map.tiles) - 1)
            y = gameworld.rng.randint(0, len(gameworld.map.tiles[x]) - 1)
            if gameworld.map.tiles[Tile.WALKABLE][x, y]:
                chosen = True
        gameworld.spawn_entity(spawn_function, (x, y))
        with pytest.raises(Impossible) as exc:
            gameworld.spawn_entity(spawn_function, (0, 0))
        assert exc.type is SpawnBlocked

    def test_determinism(self, gameworld2: GameWorld, gameworld3: GameWorld) -> None:
        assert gameworld2 == gameworld3
        for _ in range(4):
            MoveAction(gameworld2.player, (0, -1), gameworld2.map).perform()
            MoveAction(gameworld3.player, (0, -1), gameworld3.map).perform()
        assert gameworld2 == gameworld3
