from __future__ import annotations

from typing import TYPE_CHECKING, Iterable

import arcade

from actions import BumpAction
from components import ActionDelay, Position
from constants import colors, keylists
from exceptions import PathBlocked

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
        self.highlight = (0, 0)
        self.show_highlight = False
        self.floor_sprites = arcade.SpriteList()
        self.wall_sprites = arcade.SpriteList(use_spatial_hash=True)
        self.entity_sprites = arcade.SpriteList()
        self.projectile_sprites = arcade.SpriteList()

    def on_draw(self):
        self.title.draw()
        self.floor_sprites.draw()
        self.wall_sprites.draw()
        self.entity_sprites.draw()
        self.projectile_sprites.draw()
        if self.show_highlight:
            arcade.draw_lbwh_rectangle_outline(
                self.highlight[0] * 32 - 16,
                self.highlight[1] * 32 - 16,
                32,
                32,
                arcade.color.YELLOW_ORANGE,
                2,
            )

    def add_floor_sprite(self, sprite: arcade.Sprite) -> None:
        self.floor_sprites.append(sprite)

    def add_wall_sprite(self, sprite: arcade.Sprite) -> None:
        self.wall_sprites.append(sprite)

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
        projectiles: list[arcade.Sprite],
    ) -> None:
        self.set_entity_sprites(entities)
        self.set_projectile_sprites(projectiles)

    def clear_sprite_lists(self) -> None:
        self.floor_sprites.clear()
        self.wall_sprites.clear()
        self.entity_sprites.clear()
        self.projectile_sprites.clear()

    def set_camera(self):
        self.camera.position = (
            self.view.world.player.components[Position].sprite.center_x
            + self.width // 6,
            self.view.world.player.components[Position].sprite.center_y,
        )

    def on_key_press(self, symbol, modifiers):
        player_pos = self.view.world.player.components[Position]
        match symbol:
            case arcade.key.ESCAPE:
                self.view.pause_section.enabled = True
            case key if key in keylists.MOVEMENT:
                delay = self.view.world.player.components.get(ActionDelay, None)
                dir = keylists.MOVEMENT[key]
                if not self.show_highlight:
                    if delay == 0 or delay is None:
                        try:
                            BumpAction(
                                self.view.world.player,
                                dir,
                                self.view.world.map,
                                self.view.world.rng,
                            ).perform()
                            self.set_camera()
                            self.view.status_section.update_player_stats()
                        except PathBlocked:
                            self.view.message_log.add_message(
                                "The way is blocked.", colors.Impossible
                            )
                        finally:
                            self.view.message_section.update_messages()
                else:
                    dx, dy = dir
                    self.highlight = self.highlight[0] + dx, self.highlight[1] + dy
                    self.view.inspector_section.update()
            case arcade.key.H:
                if not self.show_highlight:
                    self.highlight = (player_pos.x, player_pos.y)
                    self.show_highlight = True
                    self.view.message_section.enabled = False
                    self.view.inspector_section.enabled = True
                    self.view.inspector_section.update()
                else:
                    self.show_highlight = False
                    self.view.message_section.enabled = True
                    self.view.inspector_section.enabled = False

    def on_mouse_motion(self, x, y, dx, dy):
        wx, wy, _ = self.camera.unproject((x, y))
        tx = int((wx + 16) // 32)
        ty = int((wy + 16) // 32)
        self.highlight = (tx, ty)
        self.view.inspector_section.update()
