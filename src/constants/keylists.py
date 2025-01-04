"""Keylists"""

from __future__ import annotations

from arcade import key

MOVEMENT = {
    key.NUM_1: (-1, -1),
    key.NUM_2: (0, -1),
    key.NUM_3: (1, -1),
    key.NUM_4: (-1, 0),
    key.NUM_6: (1, 0),
    key.NUM_7: (-1, 1),
    key.NUM_8: (0, 1),
    key.NUM_9: (1, 1),
    key.UP: (0, 1),
    key.DOWN: (0, -1),
    key.RIGHT: (1, 0),
    key.LEFT: (-1, 0),
    key.HOME: (-1, 1),
    key.END: (-1, -1),
    key.PAGEUP: (1, 1),
    key.PAGEDOWN: (1, -1),
}

CONFIRMATION = {
    key.RETURN,
    key.NUM_ENTER,
}
