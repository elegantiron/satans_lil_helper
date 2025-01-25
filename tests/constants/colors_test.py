from __future__ import annotations

import pytest

from constants import Colors


@pytest.mark.parametrize("color", Colors)
def test_colors(color):
        assert isinstance(color, tuple)
        for hex_value in color:
            assert isinstance(hex_value, int)
            assert 0 <= hex_value <= 255
