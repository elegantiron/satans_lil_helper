from __future__ import annotations

import time
from random import Random
from typing import TYPE_CHECKING, Generator

import numpy as np
import tcod.ecs

from gamemap import GameMap
from tile_types import ForestFloor, ForestWall
from utils import get_neighbors

if TYPE_CHECKING:
    import numpy.typing as npt

BIRTH_LIMIT = 4
DEATH_LIMIT = 3
WALL_CHANCE = 0.44
MAP_X = 100
MAP_Y = 100


class GameWorld:
    def __init__(self):
        self._current_map = GameMap()
        self._maps = [self._current_map]
        self.map_index = self._maps.index(self._current_map)
        self.rng = Random(time.time())
        tiles = [
            [(1 if self.rng.random() < WALL_CHANCE else 0) for _ in range(MAP_X)]
            for _ in range(MAP_Y)
        ]
        self.map.sprites = [[None for _ in range(MAP_X)] for _ in range(MAP_Y)]
        for _ in range(4):
            new_tiles = [[0 for _ in range(MAP_X)] for _ in range(MAP_Y)]
            for ix in range(len(tiles)):
                for iy in range(len(tiles[ix])):
                    neighbors = get_neighbors(ix, iy, tiles)
                    if neighbors > BIRTH_LIMIT:
                        new_tiles[ix][iy] = 1
            tiles = new_tiles

        self._current_map.tiles = np.full(
            (MAP_X, MAP_Y), fill_value=ForestFloor, order="F"
        )
        for ix, iy in np.ndindex(self._current_map.tiles.shape):
            if tiles[ix][iy] == 0:
                self._current_map.tiles[ix, iy] = ForestFloor
            else:
                self._current_map.tiles[ix, iy] = ForestWall
        self._current_map.new_player()

    @property
    def map(self) -> GameMap:
        return self._current_map

    @property
    def registry(self) -> tcod.ecs.Registry:
        return self.map.registry

    @property
    def player(self) -> tcod.ecs.Entity:
        return self.map.player

    @property
    def tile_list(self) -> Generator[npt.DTypeLike]:
        return self.map.tile_list
