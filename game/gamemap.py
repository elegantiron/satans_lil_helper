from __future__ import annotations

from math import ceil
from typing import TYPE_CHECKING

import numpy.typing as npt
import tcod.ecs
from pyrotkit.fov import PrecisePermissiveView
from pyrotkit.tools import Camera

from game.components import Position, Renderable, Sight
from game.constants import Sprites, TileDict
from game.maptile import MapTile

if TYPE_CHECKING:
    from pygame import Surface


class GameMap:
    def __init__(
        self,
        width: int,
        height: int,
        tile_size: int,
        screen_size: tuple[int, int],
    ):
        self.registry = tcod.ecs.Registry()
        self.player = self.registry.new_entity()
        self.tile_size = tile_size
        self.width = width
        self.height = height
        self.screen_dimensions = (
            ceil(screen_size[0] / self.tile_size),
            ceil(screen_size[1] / self.tile_size),
        )
        self.tiles: npt.NDArray = None
        self.camera = Camera(
            map=self.dims,
            screen=self.screen_dimensions,
            center=(MapTile(0, 0)),
            lock_view=True,
        )
        self.safety_calc = PrecisePermissiveView(self.dims, lambda x: False)
        self.pathfinder: tcod.path.Pathfinder = None
        self.entrance: tuple[int, MapTile] = None
        self.exit: tuple[int, MapTile] = None

    @property
    def dims(self) -> tuple[int, int]:
        return self.width, self.height

    def render(self, *, surface: Surface, sprites: dict[Sprites, Surface]):
        spriteblits = [
            (
                sprites[self.tiles[TileDict.SpriteID][ix, iy]],
                ((ix - self.camera.x_min) * 32, (iy - self.camera.y_min) * 32),
            )
            for ix in range(self.camera.x_min, self.camera.x_max)
            for iy in range(self.camera.y_min, self.camera.y_max)
            if self.tiles[TileDict.Explored][ix, iy]
        ]
        for e in self.registry.Q.all_of(components=[Renderable, Position]):
            x, y = e.components[Position].scaled()
            spriteblits.append(
                (
                    sprites[e.components[Renderable].sprite],
                    (x - (self.camera.x_min * 32), y - (self.camera.y_min * 32)),
                ),
            )
        fogblits = [
            (
                sprites[Sprites.FogTile],
                ((ix - self.camera.x_min) * 32, (iy - self.camera.y_min) * 32),
            )
            for ix in range(self.camera.x_min, self.camera.x_max)
            for iy in range(self.camera.y_min, self.camera.y_max)
            if not self.tiles[TileDict.Visible][ix, iy]
        ]

        surface.blits(spriteblits)
        surface.blits(fogblits)

    def is_walkable_tile(self, x: int, y: int) -> bool:
        return self.tiles[TileDict.Walkable][x, y]

    def setup_fov_calc(self):
        self.fov_calc = PrecisePermissiveView(
            self.dims, self.make_blocks_light_function()
        )

    def make_blocks_light_function(self):
        tiles = self.tiles

        def func(x: MapTile):
            nonlocal tiles
            return not (tiles[TileDict.Transparent][x.coords])

        return func

    def make_set_safety_function(self):
        tiles = self.tiles

        def func(x: MapTile):
            nonlocal tiles
            tiles[TileDict.Safe][x.coords] = True

        return func

    def set_safe_squares(self) -> None:
        self.safety_calc.get_view(
            MapTile(*self.player.components[Position].xy),
            15,
            self.make_set_safety_function(),
        )

    def make_visible_callback(self):
        tiles = self.tiles

        def func(x: MapTile):
            nonlocal tiles
            tiles[TileDict.Explored][*x.coords] = True
            tiles[TileDict.Visible][*x.coords] = True

        return func

    def update_player_fov(self) -> None:
        self.tiles[TileDict.Visible][:] = False
        self.fov_calc.get_view(
            center=MapTile(*self.player.components[Position].xy),
            radius=min(
                self.player.components[Sight].light,
                self.player.components[Sight].vision,
            ),
            visible_callback=self.make_visible_callback(),
        )
