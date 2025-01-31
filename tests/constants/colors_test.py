from __future__ import annotations

import pytest

from constants import Color


@pytest.mark.parametrize("color", Color)
def test_colors(color: Color) -> None:
    assert isinstance(color, tuple)
    assert 2 < len(color) < 5
    for hex_value in color:
        assert isinstance(hex_value, int)
        assert 0 <= hex_value <= 255
