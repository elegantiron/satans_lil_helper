from __future__ import annotations

import random
import time
from random import Random
from typing import TYPE_CHECKING, Generator

import arcade
import numpy as np
import tcod.ecs

from constants import TILE_SIZE, Tile
from gamemap import GameMap
from tile_types import ForestFloor, ForestWall

if TYPE_CHECKING:
    import numpy.typing as npt

MAP_X = 50
MAP_Y = 50


class GameWorld:
    _current_map: GameMap

    def __init__(self):
        self._current_map = GameMap()
        self._maps = [self._current_map]
        self.map_index = self._maps.index(self._current_map)
        tiles = [
            [(1 if random.random() < 0.48 else 0) for _ in range(MAP_X)]
            for _ in range(MAP_Y)
        ]
        self._current_map.tiles = np.full(
            (MAP_X, MAP_Y), fill_value=ForestFloor, order="F"
        )
        for ix, iy in np.ndindex(self._current_map.tiles.shape):
            if tiles[ix][iy] == 0:
                self._current_map.tiles[ix, iy] = ForestFloor
            else:
                self._current_map.tiles[ix, iy] = ForestWall
        self._current_map.new_player()
        self.rng = Random(time.time())

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

    @property
    def tile_sprites(self) -> Generator[arcade.Sprite]:
        for ix, iy in np.ndindex(self.map.tiles.shape):
            if self.map.tiles[Tile.Walkable][ix, iy]:
                yield arcade.Sprite(
                    ":images:tiles/forest/floor/000.png",
                    1,
                    ix * TILE_SIZE,
                    iy * TILE_SIZE,
                )
            else:
                yield arcade.Sprite(
                    ":images:tiles/forest/wall/000.png",
                    1,
                    ix * TILE_SIZE,
                    iy * TILE_SIZE,
                )
