from __future__ import annotations

import datetime
import random

import esper
import numpy as np
import numpy.typing as npt
import pygame

from components import Position, Renderable
from constants import DIRS, SPRITES, TILE_SIZE
import tile_types


class MapGenerator:
    def __init__(
        self, width: int = 128, height: int = 128, rng: random.Random | None = None
    ):
        self.width = width
        self.height = height
        self.rng = rng if rng else random.Random(datetime.datetime.now())

    def _new_map(self, fill):
        self.map = np.full((self.width, self.height), fill_value=fill, order="F")

    def get_new_map(self, wall, floor):
        raise NotImplementedError


class CellularGenerator(MapGenerator):
    def __init__(
        self,
        width: int = 128,
        height: int = 128,
        rng: random.Random | None = None,
        prob: float = 0.46,
    ):
        super().__init__(width, height, rng)
        self.prob = prob
        self.born = [5, 6, 7, 8]
        self.survive = [4, *self.born]

    def _randomize(self, *, floor, wall):
        """Randomizes the tiles in the given map."""
        for i in range(self.width):
            for j in range(self.height):
                self.map[i][j] = wall if self.rng.random() < self.prob else floor

    def _process(self, *, wall, floor) -> bool:
        changed = False
        neighbors = np.full(self.map.shape, fill_value=0, dtype=np.int8)
        max_x, max_y = self.map.shape
        for ix, iy in np.ndindex(self.map.shape):
            for v in DIRS:
                cx = ix + v[0]
                cy = iy + v[1]
                if (
                    not 0 <= cx < max_x
                    and not 0 <= cy < max_y
                    and self.map[cx, cy] == wall
                ):
                    neighbors[ix, iy] += 1
        for ix, iy in np.ndindex(self.map.shape):
            if self.map[ix, iy] == floor and neighbors[ix, iy] in self.born:
                self.map[ix, iy] = wall
                changed = True
            elif self.map[ix, iy] == wall and neighbors[ix, iy] not in self.survive:
                self.map[ix, iy] == floor
                changed = True
        return changed

    def get_new_map(self, wall, floor):
        self._new_map(fill=floor)
        self._randomize(floor=floor, wall=wall)
        while self._process(floor=floor, wall=wall):
            pass
        return self.map


class GameMap:
    def __init__(
        self,
        width: int,
        height: int,
        surface: pygame.Surface,
        sprites: dict[SPRITES, pygame.Surface],
        generator: MapGenerator | None = None,
    ) -> None:
        self.width = width
        self.height = height
        self.surface = surface
        self.sprites = sprites
        if generator:
            self.generator = generator
        else:
            self.generator = CellularGenerator()

        self.tiles: npt.NDArray = self.generator.get_new_map(
            wall=tile_types.forest_wall, floor=tile_types.forest_floor
        )

        self.visible = np.full(self.tiles.shape, fill_value=False, order="F")
        self.explored = np.full(self.tiles.shape, fill_value=False, order="F")
        self.fog = pygame.surface.Surface((32, 32))
        self.fog.set_alpha(0xB3)
        self.fog.fill((0, 0, 0))

    def render(self, start: tuple[int, int], stop: tuple[int, int]):
        sx, sy = start
        ex, ey = stop
        blitlist = []
        for ix, iy in np.ndindex(self.tiles[sx:ex, sy:ey].shape):
            if self.explored[ix, iy]:
                blitlist.append(
                    self.sprites[self.tiles["sprite_id"][ix + sx, iy + sy]],
                    (ix * 32, iy * 32),
                )
                if not self.visible[ix + sx, iy + sy]:
                    blitlist.append(self.fog, (ix * 32, iy * 32))
        for ent, (pos, rend) in esper.get_components(Position, Renderable):
            blitlist.append(
                (self.sprites[rend.image], (pos.x * TILE_SIZE, pos.y * TILE_SIZE))
            )

        self.surface.blits(blitlist)
        self.surface.blits(blitlist)
