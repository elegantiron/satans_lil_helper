from __future__ import annotations

import pytest

from components import Position
from constants import Tile
from gameworld import GameWorld


@pytest.fixture(scope="class")
def gameworld() -> GameWorld:
    return GameWorld()


@pytest.fixture
def gameworld2() -> GameWorld:
    return GameWorld()


class TestGameWorld:
    def test_player_position(self, gameworld: GameWorld) -> None:
        p_pos = gameworld.player.components[Position]
        assert p_pos is not None
        assert gameworld.map.tiles[Tile.WALKABLE][p_pos.xy]

    def test_equality(self, gameworld: GameWorld, gameworld2: GameWorld) -> None:
        assert gameworld != gameworld2
        assert gameworld2 == gameworld2
        assert gameworld == gameworld
