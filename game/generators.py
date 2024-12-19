from __future__ import annotations

from abc import abstractmethod, ABCMeta
import random
from typing import TYPE_CHECKING, Literal

import numpy as np

from game.constants import DIRS

if TYPE_CHECKING:
    import numpy.typing as npt

class MapGenerator(metaclass=ABCMeta):
    def _init__(self, dimensions: tuple[int, int], rng: random.Random):
        self.dimensions = dimensions
        self.rng = rng

    @property
    def width(self) -> int:
        return self.dimensions[0]
    
    @property
    def w(self) -> int:
        return self.width
    
    @property
    def h(self) -> int:
        return self.dimensions[1]
    
    @property
    def height(self) -> int:
        return self.h
    
    def fill(self, value: Literal[0, 1]):
        return np.full(shape=self.dimensions, fill_value=value, order="F", dtype=np.int8)
    
    @abstractmethod
    def generate_map(self) -> npt.NDArray:
        pass

    @staticmethod
    def get_neighbors(coords: tuple[int, int], map: npt.NDArray):
        neighbors = map[coords]
        max_x, max_y = map.shape

        for dx, dy in DIRS:
            cx = coords[0] + dx
            cy = coords[1] + dy
            if cx in range(max_x) and cy in range(max_y):
                neighbors += map[cx, cy]
            else:
                neighbors += 1
        return neighbors
    
class CellularGenerator(MapGenerator):
    pass