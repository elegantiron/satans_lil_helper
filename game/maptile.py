from __future__ import annotations

class MapTile:
    def __init__(self, x: int, y: int):
        self.x = x
        self.y = y

    @property
    def coords(self) -> tuple[int, int]:
        return self.x, self.y