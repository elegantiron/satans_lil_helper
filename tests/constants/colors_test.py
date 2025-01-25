from __future__ import annotations

from constants import Colors


def test_colors():
    for color in Colors:
        assert isinstance(color, tuple)
        for hex_value in color:
            assert isinstance(hex_value, int)