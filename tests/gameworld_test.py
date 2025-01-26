from __future__ import annotations

import pytest

from actions import MoveAction
from components import Position
from constants import Tile
from entities import enemies
from exceptions import Impossible, PathBlocked
from gameworld import GameWorld

SEED = 1737855529.0953882


@pytest.fixture(scope="class")
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

    @pytest.mark.parametrize("iteration", range(5))
    def test_entity_spawn(self, gameworld: GameWorld, iteration: int) -> None:
        entity = gameworld.spawn_entity()
        enemies.forest.wolf(entity=entity, position=(25, 25), rng=gameworld.rng)

    def test_determinism(self, gameworld2: GameWorld, gameworld3: GameWorld) -> None:
        assert gameworld2 == gameworld3
        for _ in range(4):
            MoveAction(gameworld2.player, (0, -1), gameworld2.map).perform()
            MoveAction(gameworld3.player, (0, -1), gameworld3.map).perform()
        assert gameworld2 == gameworld3
