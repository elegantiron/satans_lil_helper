"""The section for displaying the map"""

from __future__ import annotations

from typing import TYPE_CHECKING

import arcade
import numpy as np

from actions import BumpAction
from components import ActionDelay, Position, Stats
from constants import Color, Tile, keylists
from exceptions import PathBlocked

if TYPE_CHECKING:
    from collections.abc import Iterable

    import tcod.ecs

    from engine import Engine
    from gamemap import GameMap


class GameMapSection(arcade.Section):
    """Displays the map"""

    def __init__(
        self,
        left,
        bottom,
        width,
        height,
        *,
        name = None,
        accept_keyboard_keys = True,
        accept_mouse_events = True,
        prevent_dispatch = None,
        prevent_dispatch_view = None,
        local_mouse_coordinates = False,
        enabled = False,
        modal = False,
        draw_order = 1,
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
        self.highlight: tuple[int, int]
        self.show_highlight: bool
        self.tile_sprites: arcade.SpriteList
        self.entity_sprites: arcade.SpriteList
        self.projectile_sprites: arcade.SpriteList
        self.view: Engine

    def setup(self) -> None:
        """Set up the section"""
        self.camera = arcade.Camera2D(self.rect)
        self.highlight = (0, 0)
        self.show_highlight = False
        self.tile_sprites = arcade.SpriteList()
        self.entity_sprites = arcade.SpriteList()
        self.projectile_sprites = arcade.SpriteList()

    def on_draw(self) -> None:
        self.tile_sprites.draw()
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

    def set_entity_sprites(self, sprites: Iterable[arcade.Sprite]) -> None:
        """Set the list of entity sprites"""
        self.entity_sprites.clear()
        for sprite in sprites:
            self.entity_sprites.append(sprite)

    def set_projectile_sprites(self, sprites: Iterable[arcade.Sprite]) -> None:
        """Set the list of projectile sprites"""
        self.projectile_sprites.clear()
        for sprite in sprites:
            self.projectile_sprites.append(sprite)

    def clear_sprite_lists(self) -> None:
        """Remove all sprites from the sprite lists"""
        self.tile_sprites.clear()
        self.entity_sprites.clear()
        self.projectile_sprites.clear()

    def set_camera(self) -> None:
        """Set the camera position"""
        if not isinstance(self.camera, arcade.Camera2D):
            raise TypeError
        self.camera.position = arcade.Vec2(
            self.view.world.player.components[Position].sprite.center_x
            + self.width // 6,
            self.view.world.player.components[Position].sprite.center_y,
        )

    def on_key_press(self, symbol: int, modifiers: int) -> None:
        match symbol:
            case arcade.key.ESCAPE:
                self.view.pause_section.enabled = True
            case key if key in keylists.MOVEMENT:
                self.handle_move_key(key)
            case arcade.key.H:
                self.toggle_highlight()
            case arcade.key.I:
                self.view.inventory_section.enabled = True
            case arcade.key.C:
                self.view.character_section.enabled = True

    def handle_move_key(self, key: int) -> None:
        """Handle moving the player"""
        delay = self.view.player.components.get(ActionDelay, None)
        direction = keylists.MOVEMENT[key]
        if not self.show_highlight:
            if delay is None:
                delay = ActionDelay(0)
            if delay.ticks == 0:
                try:
                    BumpAction(
                        self.view.player,
                        direction,
                        self.view.map,
                        self.view.rng,
                        self.view.message_log
                    ).perform()
                    self.set_camera()
                    self.view.status_section.update_player_stats()
                    self.update_player_fov()
                    if delay.ticks == 0:
                        delay.ticks = 15
                except PathBlocked:
                    self.view.message_log.add_message(
                        "The way is blocked.", Color.IMPOSSIBLE
                    )
                finally:
                    self.view.message_section.update_messages()
        else:
            dx, dy = direction
            self.highlight = self.highlight[0] + dx, self.highlight[1] + dy
            self.view.inspector_section.update()

    def toggle_highlight(self) -> None:
        """Toggle the highlight square"""
        player_pos = self.view.player.components[Position]
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

    def on_mouse_motion(self, x: int, y: int, dx: int, dy: int) -> None:
        if not isinstance(self.camera, arcade.Camera2D):
            raise TypeError
        wx, wy, _ = self.camera.unproject((x, y))
        tx = int((wx + 16) // 32)
        ty = int((wy + 16) // 32)
        self.highlight = (tx, ty)
        self.view.inspector_section.update()

    def on_update(self, delta_time: float) -> None:
        if self.view.player.components[ActionDelay].ticks > 0:
            self.view.world.step_time()
            self.view.world.handle_regen()
            self.view.world.process_ai(self.view.message_log)
            self.view.world.handle_ailments()

    @property
    def map(self) -> GameMap:
        """The current map"""
        return self.view.map

    @property
    def player(self) -> tcod.ecs.Entity:
        """The player's entity"""
        return self.view.player

    def update_player_fov(self) -> None:
        """Update the player's FoV"""
        stats = self.player.components[Stats]
        tiles = self.map.get_fov(
            self.player.components[Position].xy, int(min(stats.light, stats.sight))
        )
        self.map.tiles[Tile.EXPLORED] |= tiles # type: ignore
        self.map.tiles[Tile.VISIBLE] = tiles # type: ignore
        i, j = np.nonzero(tiles)
        # pylint: disable=consider-using-enumerate
        for x in range(len(i)):
            self.map.sprites[i[x]][j[x]].visible = True
        # pylint: enable=consider-using-enumerate
        for ent in self.view.world.map.registry.Q.all_of(components=[Position]):
            ent.components[Position].sprite.visible = self.view.map.tiles[Tile.VISIBLE][
                ent.components[Position].xy
            ]
