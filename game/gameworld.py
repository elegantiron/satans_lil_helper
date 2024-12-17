from __future__ import annotations
import time
from typing import TYPE_CHECKING

import tcod.ecs
from pyrotkit.generators import CellularGenerator
from pyrotkit.constants import DIRS

from game.constants import Forest, Generators, Sprites
from game.definitions import tile_dt
from game.gamemap import GameMap
from game.messagelog import MessageLog
from game.utils import new_tile

if TYPE_CHECKING:
    from random import Random
    from pygame import Surface
    import pygame.freetype as freetype
    import pyrotkit.tools as pyrotools


MAP_WIDTH = 150
MAP_HEIGHT = 150

FOREST_FLOOR = new_tile(
    walkable=True,
    transparent=True,
    sprite_id=Sprites.FOREST_FLOOR,
    dtype=tile_dt,
    movement_cost=1,
)

FOREST_WALL = new_tile(
    walkable=False,
    transparent=False,
    sprite_id=Sprites.FOREST_WALL,
    dtype=tile_dt,
    movement_cost=0,
)


class GameWorld:
    def __init__(self, rng: Random, tile_size: int, screen_size: tuple[int, int]):
        self.rng = rng
        self._early_forest_gen = CellularGenerator(
            width=MAP_WIDTH,
            height=MAP_HEIGHT,
            wall=FOREST_WALL,
            floor=FOREST_FLOOR,
            dirs=DIRS.DIR8,
            rng=self.rng,
            dtype=tile_dt,
            prob=0.42,
        )
        self._maps = [
            GameMap(
                width=MAP_WIDTH,
                height=MAP_HEIGHT,
                tile_dt=tile_dt,
                floor=FOREST_FLOOR,
                wall=FOREST_WALL,
                tile_size=tile_size,
                screen_size=screen_size,
            ),
        ]
        self._early_forest_gen.rule1_iters = 4
        self._early_forest_gen.rule2_iters = 3
        self._map_index = 0
        self._tile_size = tile_size
        self._screen_size = screen_size
        self.current_map = self._maps[self._map_index]
        start = time.time()
        print(f"{start}")
        self.current_map.tiles = self._early_forest_gen.generate_map()
        print((time.time() - start))
        self.current_map.set_safe_squares()
        self.message_log = MessageLog()

    def render(
        self,
        surface: Surface,
        sprites: dict[Sprites, Surface],
        font: freetype.Font,
    ):
        self.current_map.render(surface=surface, sprites=sprites)

    @property
    def player(self) -> tcod.ecs.Entity:
        return self.current_map.player

    @property
    def camera(self) -> pyrotools.Camera:
        return self.current_map.camera

    def is_walkable_tile(self, x: int, y: int) -> bool:
        return self.current_map.is_walkable_tile(x, y)

    def new_map(self, type):
        self.current_map.exit = (self._map_index + 1, self.current_map.exit[1])
        match type:
            case Generators.EarlyForest:
                new_map = GameMap(
                    width=MAP_WIDTH,
                    height=MAP_HEIGHT,
                    tile_dt=tile_dt,
                    floor=FOREST_FLOOR,
                    wall=FOREST_WALL,
                    tile_size=self._tile_size,
                    screen_size=self._screen_size,
                )
        self._maps.append(new_map)
        self.current_map = new_map
        self.current_map.random_entrance(Forest.RandomEntranceX,Forest.RandomEntranceY)
        self.current_map.entrance = (self._map_index, self.current_map.entrance[1])