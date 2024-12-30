from __future__ import annotations

import os
from pathlib import Path
from typing import TYPE_CHECKING

import arcade
import numpy as np

from bestiary import Bestiary
from components import Position
from constants import TILE_SIZE, Tile
from gameworld import GameWorld
from messagelog import MessageLog
from sections import (
    BestiarySection,
    GameMapSection,
    InspectorSection,
    InventorySection,
    MainMenuSection,
    MessageSection,
    PauseSection,
    StatusSection,
    TitleSection,
)
from utils import load_data, save_data

if TYPE_CHECKING:
    import random

    import tcod.ecs

    from gamemap import GameMap


class Engine(arcade.View):
    world: GameWorld
    bestiary: Bestiary

    def __init__(self, window=None, background_color=None):
        super().__init__(window, background_color)

        self.message_log = MessageLog()

        self.load_satan()
        self.setup_sections()
        self.load_bestiary()

    def on_draw(self):
        self.clear()

    def load_bestiary(self):
        path = os.path.expanduser(os.path.join("~", ".slha"))
        try:
            os.makedirs(path)
        except FileExistsError:
            pass
        bestiary = Path(os.path.join(path, "bestiary.dat"))
        if bestiary.exists():
            self.bestiary = load_data(bestiary)
        else:
            self.bestiary = Bestiary()
            save_data(self.bestiary, bestiary)

    def setup_sections(self):
        self.sm = arcade.SectionManager(self)
        self.sm.enable()
        self.title_section = TitleSection(0, 0, self.width, self.height)
        self.title_section.setup(satan_sprites=self.satan_sprites)
        self.menu_section = MainMenuSection(0, 0, self.width, self.height)
        self.menu_section.setup(satan_sprites=self.satan_sprites)
        self.bestiary_section = BestiarySection(0, 0, self.width, self.height)
        self.gamemap_section = GameMapSection(0, 0, self.width, self.height)
        self.status_section = StatusSection(
            self.width * 2 / 3,
            self.height / 5,
            self.width / 3 + 5,
            self.height * 4 / 5,
            accept_keyboard_keys=False,
        )
        self.message_section = MessageSection(
            self.width * 2 / 3,
            0,
            self.width / 3 + 5,
            self.height / 5,
            accept_keyboard_keys=False,
        )

        self.pause_section = PauseSection(
            self.width / 8,
            self.height / 8,
            self.width * 6 / 8,
            self.height * 6 / 8,
        )

        self.inspector_section = InspectorSection(
            self.message_section.left,
            self.message_section.bottom,
            self.message_section.width,
            self.message_section.height,
            accept_keyboard_keys=False,
            accept_mouse_events=False,
        )

        self.inventory_section = InventorySection(
            self.pause_section.left,
            self.pause_section.bottom,
            self.pause_section.width,
            self.pause_section.height,
        )

        self.sm.add_section(self.title_section)
        self.sm.add_section(self.menu_section)
        self.sm.add_section(self.gamemap_section)
        self.sm.add_section(self.status_section)
        self.sm.add_section(self.message_section)
        self.sm.add_section(self.pause_section)
        self.sm.add_section(self.bestiary_section)
        self.sm.add_section(self.inspector_section)
        self.sm.add_section(self.inventory_section)

    def load_satan(self):
        self.satan_sprites = arcade.SpriteList()
        self.satan = {
            "main": arcade.Sprite(
                ":images:satan/main.png", 1, self.width // 2, self.height // 2
            ),
            "eyes open": arcade.Sprite(
                ":images:satan/eyes_open.png",
                1,
                self.width / 2,
                self.height / 2,
            ),
            "mouth_closed": arcade.Sprite(
                ":images:satan/mouth_closed.png",
                1,
                self.width / 2,
                self.height / 2,
            ),
            "eyes closed": arcade.Sprite(
                ":images:satan/eyes_closed.png",
                1,
                self.width / 2,
                self.height / 2,
            ),
        }
        self.satan["eyes closed"].visible = False

        self.satan_sprites.append(self.satan["main"])
        self.satan_sprites.append(self.satan["mouth_closed"])
        self.satan_sprites.append(self.satan["eyes open"])
        self.satan_sprites.append(self.satan["eyes closed"])

    def new_world(self):
        self.world = GameWorld()
        for ix, iy in np.ndindex(self.map.tiles.shape):
            if self.map.tiles[Tile.Walkable][ix, iy]:
                self.gamemap_section.add_floor_sprite(
                    arcade.Sprite(
                        ":images:tiles/forest/floor/000.png",
                        1,
                        ix * TILE_SIZE,
                        iy * TILE_SIZE,
                    )
                )
            else:
                self.gamemap_section.add_wall_sprite(
                    arcade.Sprite(
                        ":images:tiles/forest/wall/000.png",
                        1,
                        ix * TILE_SIZE,
                        iy * TILE_SIZE,
                    )
                )
        self.gamemap_section.add_entity_sprite(
            self.world.player.components[Position].sprite
        )
        self.gamemap_section.set_camera()
        self.status_section.update_player_stats()
        self.message_section.update_messages()

    @property
    def player(self) -> tcod.ecs.Entity:
        return self.world.player

    @property
    def map(self) -> GameMap:
        return self.world.map

    @property
    def registry(self) -> tcod.ecs.Registry:
        return self.world.map.registry

    @property
    def rng(self) -> random.Random:
        return self.world.rng
