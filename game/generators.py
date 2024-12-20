from __future__ import annotations

from abc import abstractmethod, ABCMeta
import random
from typing import Literal

from game.constants import DIRS


class MapGenerator(metaclass=ABCMeta):
    def __init__(self, *, dimensions: tuple[int, int], rng: random.Random):
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

    def _fill(self, value: Literal[0, 1]) -> list[list[int]]:
        return [[value for _ in range(self.height)] for _ in range(self.width)]

    @abstractmethod
    def generate_map(self) -> list[list[int]]:
        pass

    @staticmethod
    def _get_neighbors(coords: tuple[int, int], map: list[list[int]]):
        x, y = coords
        neighbors = map[x][y]
        max_x = len(map)
        max_y = len(map[0])

        for dx, dy in DIRS:
            cx = x + dx
            cy = y + dy
            if cx in range(max_x) and cy in range(max_y):
                neighbors += map[cx][cy]
            else:
                neighbors += 1
        return neighbors


class CellularGenerator(MapGenerator):
    def __init__(self, *, dimensions, rng, prob: float):
        super().__init__(dimensions=dimensions, rng=rng)
        self.prob = prob
        self.iterations = 5
        self.death_limit = 3
        self.birth_limit = 4

    def _randomize(self, grid: list[list[int]]) -> list[list[int]]:
        for x in range(len(grid)):
            for y in range(len(grid[0])):
                grid[x][y] = 1 if self.rng.random() <= self.prob else 0
        return grid

    def _process(self, grid: list[list[int]]) -> tuple[list[list[int]], bool]:
        new_grid = self._fill(0)
        changed = False
        for x in range(len(grid)):
            for y in range(len(grid[0])):
                neighbors = self._get_neighbors((x, y), grid)
                if grid[x][y] == 1:
                    if neighbors < self.death_limit:
                        new_grid[x][y] = 0
                    else:
                        new_grid[x][y] = 1
                else:
                    if neighbors > self.birth_limit:
                        new_grid[x][y] = 1
                    else:
                        new_grid[x][y] = 0
                if not changed and new_grid[x][y] != grid[x][y]:
                    changed = True
        return new_grid, changed

    def generate_map(self):
        new_map = self._fill(0)
        new_map = self._randomize(new_map)
        changed = True
        runs = 0
        while changed and runs < self.iterations:
            new_map, changed = self._process(new_map)
            runs += 1
        return new_map
