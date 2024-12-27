from __future__ import annotations

import lzma
import os
from pathlib import Path
from typing import TYPE_CHECKING

import arcade
import dill

from bestiary import Bestiary
from components import Position
from constants import Sections
from gameworld import GameWorld
from messagelog import MessageLog
from sections import (
    BestiarySection,
    GameMapSection,
    MainMenuSection,
    MessageSection,
    StatusSection,
    TitleSection,
)
from utils import load_data, save_data

if TYPE_CHECKING:
    import tcod.ecs


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
        self.title_section = TitleSection(
            0, 0, self.width, self.height, name=Sections.Title
        )
        self.title_section.setup(satan_sprites=self.satan_sprites)
        self.menu_section = MainMenuSection(
            0, 0, self.width, self.height, name=Sections.MainMenu
        )
        self.menu_section.setup(satan_sprites=self.satan_sprites)
        self.bestiary_section = BestiarySection(
            0, 0, self.width, self.height, name=Sections.Bestiary
        )
        self.gamemap_section = GameMapSection(
            0, 0, self.width, self.height, name=Sections.GameMap
        )
        self.status_section = StatusSection(
            self.width * 2 / 3,
            self.height / 5,
            self.width / 3 + 5,
            self.height * 4 / 5,
            name=Sections.Status,
            accept_keyboard_keys=False,
        )
        self.message_section = MessageSection(
            self.width * 2 / 3,
            0,
            self.width / 3 + 5,
            self.height / 5,
            name=Sections.MessageLog,
            accept_keyboard_keys=False,
        )

        self.sm.add_section(self.title_section)
        self.sm.add_section(self.menu_section)
        self.sm.add_section(self.bestiary_section)
        self.sm.add_section(self.gamemap_section)
        self.sm.add_section(self.status_section)
        self.sm.add_section(self.message_section)

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
        with open("./save.dat", "wb") as f:
            f.write(dill.dumps(self.satan))
        with open("./compressed.dat", "wb") as f:
            f.write(lzma.compress(dill.dumps(self.satan)))
        self.satan["eyes closed"].visible = False

        self.satan_sprites.append(self.satan["main"])
        self.satan_sprites.append(self.satan["mouth_closed"])
        self.satan_sprites.append(self.satan["eyes open"])
        self.satan_sprites.append(self.satan["eyes closed"])

    def new_world(self):
        self.world = GameWorld()
        self.gamemap_section.set_tile_sprites(self.world.tile_sprites)
        self.gamemap_section.add_entity_sprite(
            self.world.player.components[Position].sprite
        )
        self.gamemap_section.set_camera()
        player_pos = self.world.player.components[Position]
        self.status_section.player_location.text = (
            f"Location: {player_pos.x},{player_pos.y}"
        )

    @property
    def player(self) -> tcod.ecs.Entity:
        return self.world.player