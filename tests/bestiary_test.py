from __future__ import annotations

import pytest

from bestiary import Bestiary
from constants import tags

# pylint: disable=redefined-outer-name


@pytest.fixture(scope="class")
def bestiary() -> Bestiary:
    return Bestiary()


@pytest.fixture(scope="class")
def bestiary2() -> Bestiary:
    return Bestiary()


class TestBestiary:
    def test_kills(self, bestiary: Bestiary, bestiary2: Bestiary) -> None:
        for enemy_type in list(tags.Enemies):
            for _ in range(50):
                bestiary.record_kill(enemy_type=enemy_type, alpha=False)
                bestiary2.record_kill(enemy_type=enemy_type, alpha=False)
            for _ in range(25):
                bestiary.record_kill(enemy_type=enemy_type, alpha=True)
                bestiary2.record_kill(enemy_type=enemy_type, alpha=True)

    def test_get_stats(self, bestiary: Bestiary) -> None:
        assert bestiary.total_murders() == 75 * len(tags.Enemies)
        for enemy_type in list(tags.Enemies):
            assert bestiary.total_murders(enemy_type) == 75

    def test_comparison(self, bestiary: Bestiary, bestiary2: Bestiary) -> None:
        assert bestiary == bestiary2
