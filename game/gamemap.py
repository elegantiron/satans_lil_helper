from __future__ import annotations
from typing import TYPE_CHECKING

import numpy as np
import numpy.typing as npt
import pygame.display as pdisp
from pyrotkit.tools import Camera
from pyrotkit import types as pyrotypes
import tcod.ecs

from game.components import Position, Renderable
from game.constants import Tile, Professions
from game.entity_factories import make_player

if TYPE_CHECKING:
    from pygame import Surface
    from constants import Sprites


class GameMap:
    def __init__(
        self,
        width: int,
        height: int,
        tile_dt: npt.DTypeLike,
        floor,
        wall,
        tile_size: int,
    ):
        self.registry = tcod.ecs.Registry()
        self.player = self.registry.new_entity()
        self.tile_size = tile_size
        size = pdisp.get_window_size()
        self.screen_dimensions = (size[0] // self.tile_size, size[1] // self.tile_size)
        make_player(self.player, Professions.Warrior, (0, 0))
        self.tiles = np.full(
            shape=(width, height), dtype=tile_dt, order="F", fill_value=wall
        )
        self.camera = Camera(
            map=(width, height),
            screen=self.screen_dimensions,
            center=(
                pyrotypes.Tile(
                    pyrotypes.Coordinate(*self.player.components[Position].xy)
                )
            ),
            lock_view=True,
        )


    def render(self, *, surface: Surface, sprites: dict[Sprites, Surface]):
        blitlist = [
            (
                sprites[self.tiles[Tile.SpriteID][ix, iy]],
                ((ix - self.camera.x_min) * 32, (iy - self.camera.y_min) * 32),
            )
            for ix in range(self.camera.x_min, self.camera.x_max)
            for iy in range(self.camera.y_min, self.camera.y_max)
        ]
        for e in self.registry.Q.all_of(components=[Renderable, Position]):
            x, y = e.components[Position].scaled()
            blitlist.append(
                (
                    sprites[e.components[Renderable].sprite],
                    (x - (self.camera.x_min * 32), y - (self.camera.y_min * 32)),
                ),
            )
        surface.blits(blitlist)

    def is_walkable_tile(self, x: int, y: int) -> bool:
        return self.tiles[Tile.Walkable][x, y]
