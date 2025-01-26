from __future__ import annotations

import pytest

from components import Stats


@pytest.fixture
def component() -> Stats:
    return Stats(5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5)

@pytest.fixture
def math_stats() -> Stats:
    return Stats(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1)


class TestStats:
    def test_creation(self, component: Stats) -> None:
        assert component.hp == 5
        assert component.max_hp == 5
        assert component.mp == 5
        assert component.max_mp == 5
        assert component.strength == 5
        assert component.magic == 5
        assert component.pdef == 5
        assert component.mdef == 5
        assert component.evasion == 5
        assert component.crit == 5
        assert component.sight == 5
        assert component.light == 5
        assert component.level == 5
        assert component.xp == 5
        assert component.xp_granted == 5

    def test_setting_hp(self, component: Stats) -> None:
        component.hp = 3
        assert component.hp == 3
        component.hp += 1
        assert component.hp == 4
        component.hp = 6
        assert component.hp == 5

    def test_setting_mp(self, component: Stats) -> None:
        component.mp = 3
        assert component.mp == 3
        component.mp += 1
        assert component.mp == 4
        component.mp = 6
        assert component.mp == 5

    def test_addition(self, component: Stats, math_stats: Stats) -> None:
        test_component = component + math_stats
        assert test_component.hp == 6
        assert test_component.max_hp == 6
        assert test_component.mp == 6
        assert test_component.max_mp == 6
        assert test_component.strength == 6
        assert test_component.magic == 6
        assert test_component.pdef == 6
        assert test_component.mdef == 6
        assert test_component.evasion == 6
        assert test_component.crit == 6
        assert test_component.sight == 6
        assert test_component.light == 6


    def test_subtraction(self, component: Stats, math_stats: Stats) -> None:
        test_component = component - math_stats
        assert test_component.hp == 4
        assert test_component.max_hp == 4
        assert test_component.mp == 4
        assert test_component.max_mp == 4
        assert test_component.strength == 4
        assert test_component.magic == 4
        assert test_component.pdef == 4
        assert test_component.mdef == 4
        assert test_component.evasion == 4
        assert test_component.crit == 4
        assert test_component.sight == 4
        assert test_component.light == 4
