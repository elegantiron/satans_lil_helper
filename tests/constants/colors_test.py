from __future__ import annotations

import pytest

from constants import Colors


@pytest.mark.parametrize("color", Colors)
def test_colors(color: Colors) -> None:
        assert isinstance(color, tuple)
        assert 2 < len(color) < 5
        for hex_value in color:
            assert isinstance(hex_value, int)
            assert 0 <= hex_value <= 255
