from __future__ import annotations

from typing import TYPE_CHECKING

import numpy as np
import tcod.ecs
from pygame import Rect

from game.components import Position, Renderable, Sight, Health, Mana
from game.constants import Forest, Generators, Sprites, TileDict, Strings
from game.definitions import tile_dt
from game.gamemap import GameMap
from game.generators import CellularGenerator
from game.messagelog import MessageLog
from game.utils import write_centered

if TYPE_CHECKING:
    from random import Random

    import pygame.freetype as freetype
    import pyrotkit.tools as pyrotools
    from pygame import Surface


class GameWorld:
    def __init__(self, rng: Random, tile_size: int, screen_size: tuple[int, int]):
        self.rng = rng
        self._early_forest_gen = CellularGenerator(
            dimensions=(Forest.Width, Forest.Height),
            rng=self.rng,
            prob=Forest.EarlyProb,
        )
        game_screen = (screen_size[0] * 2 / 3, screen_size[1])
        self.current_map = GameMap(
            width=Forest.Width,
            height=Forest.Height,
            tile_size=tile_size,
            screen_size=game_screen,
        )
        self._maps = [self.current_map]
        self.map_index = self._maps.index(self.current_map)
        self._tile_size = tile_size
        self._screen_size = screen_size
        self.current_map.tiles = np.ndarray(
            (Forest.Width, Forest.Height), dtype=tile_dt, order="F"
        )
        temp_map = self._early_forest_gen.generate_map()
        for ix, iy in np.ndindex(self.current_map.tiles.shape):
            self.current_map.tiles[ix, iy] = (
                Forest.Wall if temp_map[ix][iy] == 1 else Forest.Floor
            )
        picked = False
        while not picked:
            x = rng.randint(0, Forest.Width)
            y = rng.randint(0, 15)
            if self.current_map.tiles[TileDict.Walkable][x, y]:
                self.player.components[Position] = Position(x, y)
                picked = True
        # self.current_map.set_safe_squares()
        self.player.components[Sight] = Sight(7, 7)
        self.message_log = MessageLog()
        self.current_map.camera.set_center(*self.player.components[Position].xy)
        self.current_map.setup_fov_calc()
        self.current_map.update_player_fov()
        self.player.components |= {
            Renderable: Renderable(Sprites.Player),
            Health: Health(30),
            Mana: Mana(5),
        }
        self.rects = {
            "message_box": Rect(
                screen_size[0] * 2 // 3,
                screen_size[1] * 4 // 5,
                screen_size[0] // 2,
                screen_size[1] // 5,
            )
        }

    def render(
        self,
        surface: Surface,
        working_surface: Surface,
        sprites: dict[Sprites, Surface],
        font: freetype.Font,
    ):
        self.current_map.render(surface=surface, sprites=sprites)
        self.message_log.render(
            surface=surface, rect=self.rects["message_box"], font=font
        )
        self.render_status(surface, font)

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

    def render_status(self, surface: Surface, font: freetype.Font) -> None:
        dest = Rect(surface.width * 2 // 3, 5, surface.width // 3, surface.height)
        rect = write_centered(
            surface=surface, font=font, text=Strings.Status, rect=dest
        )
        dest.top = dest.top + rect.height + 5
        text = f"Location: {self.player.components[Position].x},{self.player.components[Position].y}"
        rect = font.render_to(
            surf=surface,
            text=text,
            dest=dest,
        )
        dest.top = dest.top + rect.height + 5
        text = f"Health: {self.player.components[Health].hp}/{self.player.components[Health].max_hp}\t\tMana: {self.player.components[Mana].mp}/{self.player.components[Mana].max_mp}"
        rect = font.render_to(
            surf=surface,
            text=text,
            dest=dest,
        )
