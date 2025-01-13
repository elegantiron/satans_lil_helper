"""Main game engine"""

from __future__ import annotations

import contextlib
from pathlib import Path
from typing import TYPE_CHECKING

import arcade
import numpy as np

from ai_helpers import confused_action, hostile_action, wander_action
from bestiary import Bestiary
from components import AI, ActionDelay, Confusion, Position
from constants import TILE_SIZE, AIType, Tile
from entities import enemies
from exceptions import Impossible
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
    """Handles coordinating the game pieces."""

    world: GameWorld
    bestiary: Bestiary

    def __init__(
        self,
        window: arcade.Window = None,
        background_color: tuple[int, int, int, int] = None,
    ) -> None:
        super().__init__(window, background_color)

        self.message_log = MessageLog()

        self.load_satan()
        self.setup_sections()
        self.load_bestiary()

    def on_draw(self) -> None:
        self.clear()

    def load_bestiary(self) -> None:
        """Load or create a bestiary"""
        path = Path.expanduser(Path("~") / ".slha")
        with contextlib.suppress(FileExistsError):
            path.mkdir(parents=True)
        bestiary = path / "bestiary.dat"
        if bestiary.exists():
            self.bestiary = load_data(bestiary)
        else:
            self.bestiary = Bestiary()
            save_data(self.bestiary, bestiary)

    def setup_sections(self) -> None:
        """Set up the sections"""
        self.sm = arcade.SectionManager(self)
        self.sm.enable()
        self.title_section = TitleSection(0, 0, self.width, self.height)

        self.menu_section = MainMenuSection(0, 0, self.width, self.height)

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

        self.title_section.setup()
        self.menu_section.setup()
        self.gamemap_section.setup()
        self.bestiary_section.setup()
        self.status_section.setup()
        self.message_section.setup()
        self.pause_section.setup()
        self.inspector_section.setup()
        self.inventory_section.setup()

    def load_satan(self) -> None:
        """Load the sprites for Satan"""
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

    def new_world(self) -> None:
        """Make a new world"""
        self.world = GameWorld()
        for ix, iy in np.ndindex(self.map.tiles.shape):
            if self.map.tiles[Tile.WALKABLE][ix, iy]:
                self.world.map.sprites[ix][iy] = arcade.Sprite(
                    ":images:tiles/forest/floor/000.png",
                    1,
                    ix * TILE_SIZE,
                    iy * TILE_SIZE,
                    visible=False,
                )
            else:
                self.world.map.sprites[ix][iy] = arcade.Sprite(
                    ":images:tiles/forest/wall/000.png",
                    1,
                    ix * TILE_SIZE,
                    iy * TILE_SIZE,
                    visible=False,
                )
        for sprites in self.world.map.sprites:
            for sprite in sprites:
                self.gamemap_section.tile_sprites.append(sprite)
        self.gamemap_section.entity_sprites.append(
            self.world.player.components[Position].sprite
        )
        self.gamemap_section.set_camera()
        self.status_section.update_player_stats()
        self.message_section.update_messages()
        for _ in range(25):
            wolf = self.map.registry.new_entity()
            chosen = False
            x = None
            y = None
            while not chosen:
                x = self.rng.choice(range(self.world.MAP_X))
                y = self.rng.choice(range(self.world.MAP_Y))
                if self.map.tiles[Tile.WALKABLE][x, y]:
                    chosen = True
            enemies.forest.wolf(position=(x, y), entity=wolf, rng=self.rng)
            self.gamemap_section.entity_sprites.append(wolf.components[Position].sprite)
        self.gamemap_section.update_player_fov()

    @property
    def player(self) -> tcod.ecs.Entity:
        """The player's entity"""
        return self.world.player

    @property
    def map(self) -> GameMap:
        """The current world"""
        return self.world.map

    @property
    def registry(self) -> tcod.ecs.Registry:
        """The active registry"""
        return self.world.map.registry

    @property
    def rng(self) -> random.Random:
        """The RNG"""
        return self.world.rng

    def process_tick(self) -> None:
        """Process turns for all entities."""
        for ent in self.registry.Q.all_of(components=[Position, AI]):
            action_delay = ent.components.get(ActionDelay, None)
            if action_delay is None:
                pass
            elif action_delay.ticks > 0:
                action_delay.ticks -= 1
            else:
                try:
                    match ent.components[AI].type:
                        case AIType.WANDERING:
                            wander_action(ent, self.map, self.rng)
                        case AIType.CONFUSED:
                            confusion = ent.components.get(Confusion, None)
                            if confusion.turns < confusion.limit:
                                confused_action(ent, self.map, self.rng)
                                confusion.turns += 1
                            else:
                                ent.components[AI].type = ent.components[AI].base_type
                        case AIType.HOSTILE:
                            hostile_action(ent, self.map, self.rng)
                        case AIType.HOWL_RESPONSE:
                            pass
                except Impossible:
                    # Catch impossible actions and ignore them.
                    # We don't care if the AI tries something it can't do
                    pass
        self.gamemap_section.update_player_fov()
        self.player.components[ActionDelay].ticks -= 1
