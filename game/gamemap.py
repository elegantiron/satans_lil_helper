from __future__ import annotations
from math import ceil
from typing import TYPE_CHECKING

import numpy.typing as npt
from pyrotkit.fov import PrecisePermissiveView
from pyrotkit.tools import Camera
import tcod.ecs

from game.components import Position, Renderable
from game.constants import TileDict
from game.maptile import MapTile

if TYPE_CHECKING:
    from pygame import Surface
    from constants import Sprites


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
            center=(MapTile(*self.player.components[Position].xy)),
            lock_view=True,
        )
        self.fov_calc = PrecisePermissiveView(
            self.dims, self.make_passes_light_function()
        )
        self.safety_calc = PrecisePermissiveView(self.dims, lambda x: False)
        self.pathfinder: tcod.path.Pathfinder = None
        self.entrance: tuple[int, MapTile] = None
        self.exit: tuple[int, MapTile] = None

    @property
    def dims(self) -> tuple[int, int]:
        return self.width, self.height

    def render(self, *, surface: Surface, sprites: dict[Sprites, Surface]):
        blitlist = [
            (
                sprites[self.tiles[TileDict.SpriteID][ix, iy]],
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
        return self.tiles[TileDict.Walkable][x, y]

    def make_passes_light_function(self):
        tiles = self.tiles

        def func(x: MapTile):
            nonlocal tiles
            return not tiles[TileDict.Transparent][x.coords]

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

    def setup_pathfinder(self) -> None:
        graph = tcod.path.SimpleGraph(
            cost=self.tiles[TileDict.MovementCost], cardinal=2, diagonal=3
        )
        self.pathfinder = tcod.path.Pathfinder(graph)

    def reset_pathfinder(self) -> None:
        if self.pathfinder is None:
            self.setup_pathfinder()
        self.pathfinder.clear()

    def add_pathfinder_root(self, target: MapTile) -> None:
        if self.pathfinder is None:
            self.setup_pathfinder()
        self.pathfinder.add_root(target.coords)

    def get_path_to(self, target: MapTile) -> list[tuple[int, int]]:
        if self.pathfinder is None:
            self.setup_pathfinder()
        return self.pathfinder.path_to(target.coords)[1:].tolist()

    def get_path_from(self, start: MapTile) -> list[tuple[int, int]]:
        if self.pathfinder is None:
            self.setup_pathfinder()
        return self.pathfinder.path_from(start.coords)[1:].tolist()
