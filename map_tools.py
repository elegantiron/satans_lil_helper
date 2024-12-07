from __future__ import annotations

import datetime
import random

import numpy as np

from constants import DIRS


class MapGenerator:
    def __init__(
        self, width: int = 128, height: int = 128, rng: random.Random | None = None
    ):
        self.width = width
        self.height = height
        self.rng = rng if rng else random.Random(datetime.datetime.now())

    def _new_map(self, fill):
        self.map = np.full((self.width, self.height), fill_value=fill, order="F")

    def get_new_map(self, wall, floor):
        raise NotImplementedError


class CellularGenerator(MapGenerator):
    def __init__(
        self,
        width: int = 128,
        height: int = 128,
        rng: random.Random | None = None,
        prob: float = 0.46,
    ):
        super().__init__(width, height, rng)
        self.prob = prob
        self.born = [5, 6, 7, 8]
        self.survive = [4, *self.born]

    def _randomize(self, *, floor, wall):
        """Randomizes the tiles in the given map."""
        for i in range(self.width):
            for j in range(self.height):
                self.map[i][j] = wall if self.rng.random() < self.prob else floor

    def _process(self, *, wall, floor) -> bool:
        changed = False
        neighbors = np.full(self.map.shape, fill_value=0, dtype=np.int8)
        max_x, max_y = self.map.shape
        for ix, iy in np.ndindex(self.map.shape):
            for v in DIRS:
                cx = ix + v[0]
                cy = iy + v[1]
                if (
                    not 0 <= cx < max_x
                    and not 0 <= cy < max_y
                    and self.map[cx, cy] == wall
                ):
                    neighbors[ix, iy] += 1
        for ix, iy in np.ndindex(self.map.shape):
            if self.map[ix, iy] == floor and neighbors[ix, iy] in self.born:
                self.map[ix, iy] = wall
                changed = True
            elif self.map[ix, iy] == wall and neighbors[ix, iy] not in self.survive:
                self.map[ix, iy] == floor
                changed = True
        return changed

    def get_new_map(self, wall, floor):
        self._new_map(fill=floor)
        self._randomize(floor=floor, wall=wall)
        while self._process(floor=floor, wall=wall):
            pass
        return self.map

