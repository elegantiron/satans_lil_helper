"""Tools for handling maps, mostly generators."""

from __future__ import annotations

import time
import random

import numpy as np

from constants import DIRS


class MapGenerator:
    """Base class for generating maps.
    
    :param width: Width of the maps to generate.
    :param height: Height of the maps to generate.
    :param rng: Random generator to use for generating maps.
    """
    def __init__(
        self, width: int = 128, height: int = 128, rng: random.Random | None = None
    ):
        self.width = width
        self.height = height
        self.rng = rng if rng else random.Random(int(time.time()))

    def _new_map(self, fill) ->None:
        self.map = np.full((self.width, self.height), fill_value=fill, order="F")

    def get_new_map(self):
        """Returns a new map object.
        
        Subclasses must override this."""
        raise NotImplementedError


class CellularGenerator(MapGenerator):
    """Generates maps with an algorithm based on Conway's Game of Life.

    :param width: Width of the maps to generate.
    :param height: Height of the maps to generate.
    :param rng: Random generator to use for generating maps.
    :param prob: Probability that any given tile will be a wall when randomizing.
    """
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

    def _randomize(self):
        for ix, iy in np.ndindex(self.map.shape):
                self.map[ix][iy] = 1 if self.rng.random() < self.prob else 0

    def _process(self) -> bool:
        changed = False
        neighbors = np.full(self.map.shape, fill_value=0, dtype=np.int8)
        max_x, max_y = self.map.shape
        for ix, iy in np.ndindex(self.map.shape):
            for v in DIRS:
                cx = ix + v[0]
                cy = iy + v[1]
                if not 0 <= cx < max_x and not 0 <= cy < max_y:
                    neighbors[ix, iy] += 1
                elif not 0 <= cx < max_x or not 0 <= cy < max_y:
                    neighbors[ix, iy] += 1
                else:
                    neighbors[ix, iy] += self.map[ix, iy]
        for ix, iy in np.ndindex(self.map.shape):
            if self.map[ix, iy] == 0 and neighbors[ix, iy] in self.born:
                self.map[ix, iy] = 1
                changed = True
            elif self.map[ix, iy] == 1 and neighbors[ix, iy] not in self.survive:
                self.map[ix, iy] == 0
                changed = True
        return changed

    def get_new_map(self):
        """Returns a new cellular map.

        Takes care of clearing and randomizing the tiles
        before appling the CGOL algorithm to them.
        
        :rtype: list[list[int]]"""
        self._new_map(fill=0)
        self._randomize()
        while self._process():
            pass
        return self.map
