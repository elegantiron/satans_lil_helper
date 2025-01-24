from __future__ import annotations

import pytest

from bestiary import Bestiary


@pytest.fixture(scope="class")
def bestiary() -> Bestiary:
    return Bestiary()


@pytest.fixture(scope="class")
def bestiary2() -> Bestiary:
    return Bestiary()


class TestBestiary:
    @pytest.mark.xfail(reason="test not written", raises=NotImplementedError)
    def test_kills(self, bestiary):
        raise NotImplementedError

    @pytest.mark.xfail(reason="test not written", raises=NotImplementedError)
    def test_get_stats(self, bestiary):
        raise NotImplementedError

    def test_comparison(self, bestiary, bestiary2):
        assert bestiary == bestiary2
