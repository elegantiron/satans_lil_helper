from __future__ import annotations
from typing import TYPE_CHECKING

import tcod.ecs
from pyrotkit.generators import CellularGenerator
from pyrotkit.constants import DIRS

from game.constants import Sprites
from game.definitions import tile_dt
from game.gamemap import GameMap
from game.messagelog import MessageLog
from game.utils import new_tile

if TYPE_CHECKING:
    from random import Random
    from pygame import Surface
    import pygame.freetype as freetype
    import pyrotkit.tools as pyrotools


MAP_WIDTH = 75
MAP_HEIGHT = 75

FOREST_FLOOR = new_tile(
    walkable=True, transparent=True, sprite_id=Sprites.FOREST_FLOOR, dtype=tile_dt
)

FOREST_WALL = new_tile(
    walkable=False, transparent=False, sprite_id=Sprites.FOREST_WALL, dtype=tile_dt
)


class GameWorld:
    def __init__(self, rng: Random, tile_size: int):
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
                tile_size=tile_size
            ),
        ]
        self._early_forest_gen.rule1_iters = 4
        self._early_forest_gen.rule2_iters = 5
        self._current_map = self._maps[0]
        self._current_map.tiles = self._early_forest_gen.generate_map()
        self.message_log = MessageLog()

    def render(
        self,
        surface: Surface,
        sprites: dict[Sprites, Surface],
        font: freetype.Font,
    ):
        self._current_map.render(surface=surface, sprites=sprites)

    @property
    def player(self) -> tcod.ecs.Entity:
        return self._current_map.player
    
    @property
    def camera(self) -> pyrotools.Camera:
        return self._current_map.camera

    def is_walkable_tile(self, x:int, y:int)->bool:
        return self._current_map.is_walkable_tile(x, y)