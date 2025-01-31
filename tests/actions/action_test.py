from __future__ import annotations

import pytest

from actions import MoveAction
from exceptions import Impossible, OutOfBounds, PathBlocked
from gameworld import GameWorld

SEED = 1737855529.0953882


# pylint: disable=redefined-outer-name
@pytest.fixture(scope="class")
def gameworld() -> GameWorld:
    return GameWorld(SEED)


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
