from __future__ import annotations

from typing import TYPE_CHECKING, Iterable

import arcade

from components import Position
from constants import keylists

if TYPE_CHECKING:
    from engine import Engine


class GameMapSection(arcade.Section):
    camera: arcade.Camera2D
    view: Engine

    def __init__(
        self,
        left,
        bottom,
        width,
        height,
        *,
        name=None,
        accept_keyboard_keys=True,
        accept_mouse_events=True,
        prevent_dispatch=None,
        prevent_dispatch_view=None,
        local_mouse_coordinates=False,
        enabled=False,
        modal=False,
        draw_order=1,
    ):
        super().__init__(
            left,
            bottom,
            width,
            height,
            name=name,
            accept_keyboard_keys=accept_keyboard_keys,
            accept_mouse_events=accept_mouse_events,
            prevent_dispatch=prevent_dispatch,
            prevent_dispatch_view=prevent_dispatch_view,
            local_mouse_coordinates=local_mouse_coordinates,
            enabled=enabled,
            modal=modal,
            draw_order=draw_order,
        )
        self.camera = arcade.Camera2D(self.rect)
        self.title = arcade.Text(
            "Map", self.width // 2, self.height - 10, anchor_x="center", anchor_y="top"
        )
        self.tile_sprites = arcade.SpriteList()
        self.entity_sprites = arcade.SpriteList()
        self.projectile_sprites = arcade.SpriteList()

    def on_draw(self):
        self.title.draw()
        self.tile_sprites.draw()
        self.entity_sprites.draw()
        self.projectile_sprites.draw()

    def set_tile_sprites(self, sprites: Iterable[arcade.Sprite]) -> None:
        self.tile_sprites.clear()
        for sprite in sprites:
            self.tile_sprites.append(sprite)

    def set_entity_sprites(self, sprites: Iterable[arcade.Sprite]) -> None:
        self.entity_sprites.clear()
        for sprite in sprites:
            self.entity_sprites.append(sprite)

    def set_projectile_sprites(self, sprites: Iterable[arcade.Sprite]) -> None:
        self.projectile_sprites.clear()
        for sprite in sprites:
            self.projectile_sprites.append(sprite)

    def add_entity_sprite(self, sprite: arcade.Sprite) -> None:
        self.entity_sprites.append(sprite)

    def set_sprites(
        self,
        *,
        entities: list[arcade.Sprite],
        tiles: list[arcade.Sprite],
        projectiles: list[arcade.Sprite],
    ) -> None:
        self.set_entity_sprites(entities)
        self.set_tile_sprites(tiles)
        self.set_projectile_sprites(projectiles)

    def clear_sprite_lists(self) -> None:
        self.tile_sprites.clear()
        self.entity_sprites.clear()
        self.projectile_sprites.clear()

    def set_camera(self):
        self.camera.position = (
            self.view.world.player.components[Position].sprite.center_x
            + self.width // 6,
            self.view.world.player.components[Position].sprite.center_y,
        )

    def on_key_press(self, symbol, modifiers):
        match symbol:
            case arcade.key.ESCAPE:
                self.view.pause_section.enabled = True
            case key if key in keylists.MOVEMENT:
                player_pos = self.view.world.player.components[Position]
                dx, dy = keylists.MOVEMENT[key]
                player_pos.x = player_pos.x + dx
                player_pos.y = player_pos.y + dy
                self.set_camera()
                self.view.status_section.update_player_stats()
