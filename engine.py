from __future__ import annotations

import lzma
import pickle
from typing import Dict, TYPE_CHECKING

from tcod.map import compute_fov

import exceptions
from message_log import MessageLog
from pygame import freetype, mouse
import render_functions


if TYPE_CHECKING:
    from entity import Actor
    from game_map import GameMap, GameWorld
    from pygame import Surface


class Engine:
    game_map: GameMap
    game_world: GameWorld

    def __init__(self, player: Actor):
        self.message_log = MessageLog()
        self.mouse_location = (0, 0)
        self.player = player

    def handle_enemy_turns(self) -> None:
        for entity in set(self.game_map.actors) - {self.player}:
            if entity.ai:
                try:
                    entity.ai.perform()
                except exceptions.Impossible:
                    pass  # Ignore impossible actions exceptions from the AI

    def handle_status_effects(self) -> None:
        for entity in set(self.game_map.actors):
            if entity.status_effects:
                try:
                    for effect in entity.status_effects:
                        effect.proc()
                except exceptions.Impossible:
                    pass  # Ignore when this messes up

    def update_fov(self) -> None:
        """Recompute the visible area based on the player's point of view."""
        self.game_map.visible[:] = compute_fov(
            self.game_map.tiles["transparent"],
            (self.player.x, self.player.y),
            radius=8,
        )
        self.game_map.explored |= self.game_map.visible

    def render(
        self,
        surface: Surface,
        tile_images: Dict[str, Surface],
        entity_sprites: Dict[str, Surface],
        font: freetype.Font,
    ) -> None:
        self.game_map.render(surface, tile_images, entity_sprites)
        # render_functions.render_bar(
        #     surface=surface,
        #     current_value=self.player.fighter.hp,
        #     maximum_value=self.player.fighter.max_hp,
        #     total_width=int(STATUS.WIDTH * 0.7),
        # )
        render_functions.render_game_status(
            surface=surface,
            dungeon_level=self.game_world.current_floor,
            player=self.player,
            font=font,
        )
        self.message_log.render(
            surface=surface, font=font, x=21, y=45, width=35, height=13
        )
        x, y = mouse.get_pos()
        render_functions.render_names_at_mouse_location(
            surface=surface,
            font=font,
            x=x + 13,
            y=y,
            engine=self,
        )

    def save_as(self, filename: str) -> None:
        """Save this Engine instance as a compressed file."""
        save_data = lzma.compress(pickle.dumps(self))
        with open(filename, "wb") as f:
            f.write(save_data)
