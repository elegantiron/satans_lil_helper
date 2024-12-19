from __future__ import annotations
from typing import TYPE_CHECKING

import tcod.ecs
from pyrotkit.generators import CellularGenerator
from pyrotkit.constants import DIRS

from game.components import Position
from game.constants import Forest, Generators, Sprites, TileDict
from game.definitions import tile_dt
from game.gamemap import GameMap
from game.messagelog import MessageLog

if TYPE_CHECKING:
    from random import Random
    from pygame import Surface
    import pygame.freetype as freetype
    import pyrotkit.tools as pyrotools


class GameWorld:
    def __init__(self, rng: Random, tile_size: int, screen_size: tuple[int, int]):
        self.rng = rng
        self._early_forest_gen = CellularGenerator(
            width=Forest.Width,
            height=Forest.Height,
            wall=Forest.Wall,
            floor=Forest.Floor,
            dirs=DIRS.DIR8,
            rng=self.rng,
            dtype=tile_dt,
            prob=Forest.EarlyProb,
        )
        self._early_forest_gen.rule1_iters = 4
        self._early_forest_gen.rule2_iters = 3
        self.current_map = GameMap(
            width=Forest.Width,
            height=Forest.Height,
            tile_size=tile_size,
            screen_size=screen_size,
        )
        self._maps = [self.current_map]
        self.map_index = self._maps.index(self.current_map)
        self._tile_size = tile_size
        self._screen_size = screen_size
        self.current_map.tiles = self._early_forest_gen.generate_map()
        picked = False
        while not picked:
            x = rng.randint(0, Forest.Width)
            y = rng.randint(0, 15)
            if self.current_map.tiles[TileDict.Walkable][x, y]:
                self.player.components[Position] = Position(x, y)
                picked = True
        # self.current_map.set_safe_squares()
        self.message_log = MessageLog()

    def render(
        self,
        surface: Surface,
        working_surface: Surface,
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
        raise NotImplementedError
        self.current_map.exit = (self.map_index + 1, self.current_map.exit[1])
        match type:
            case Generators.EarlyForest:
                new_map = GameMap(
                    width=Forest.Width,
                    height=Forest.Height,
                    tile_size=self._tile_size,
                    screen_size=self._screen_size,
                )
        self._maps.append(new_map)
        self.current_map = new_map
