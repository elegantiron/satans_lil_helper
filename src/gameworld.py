"""The entire world"""

from __future__ import annotations

import time
from random import Random

import numpy as np
import tcod.ecs

from gamemap import GameMap
from tile_types import ForestFloor, ForestWall
from utils import get_neighbors


class GameWorld:
    """The entire world, and the methods to make new maps"""

    BIRTH_LIMIT = 4
    DEATH_LIMIT = 3
    WALL_CHANCE = 0.44
    MAP_X = 100
    MAP_Y = 100

    def __init__(self):
        self._current_map = GameMap()
        self._maps = [self._current_map]
        self.map_index = self._maps.index(self._current_map)
        self.rng = Random(time.time())
        tiles = [
            [
                (1 if self.rng.random() < self.WALL_CHANCE else 0)
                for _ in range(self.MAP_X)
            ]
            for _ in range(self.MAP_Y)
        ]
        self.map.sprites = [
            [None for _ in range(self.MAP_X)] for _ in range(self.MAP_Y)
        ]
        for _ in range(4):
            new_tiles = [[0 for _ in range(self.MAP_X)] for _ in range(self.MAP_Y)]
            # pylint: disable=consider-using-enumerate
            for ix in range(len(tiles)):
                for iy in range(len(tiles[ix])):
                    neighbors = get_neighbors(ix, iy, tiles)
                    if neighbors > self.BIRTH_LIMIT:
                        new_tiles[ix][iy] = 1
            # pylint: enable=consider-using-enumerate
            tiles = new_tiles

        self._current_map.tiles = np.full(
            (self.MAP_X, self.MAP_Y), fill_value=ForestFloor, order="F"
        )
        for ix, iy in np.ndindex(self._current_map.tiles.shape):
            if tiles[ix][iy] == 0:
                self._current_map.tiles[ix, iy] = ForestFloor
            else:
                self._current_map.tiles[ix, iy] = ForestWall
        self._current_map.new_player()

    @property
    def map(self) -> GameMap:
        """The current map"""
        return self._current_map

    @property
    def registry(self) -> tcod.ecs.Registry:
        """The active registry"""
        return self.map.registry

    @property
    def player(self) -> tcod.ecs.Entity:
        """The player entity"""
        return self.map.player
