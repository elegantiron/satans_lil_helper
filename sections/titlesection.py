from __future__ import annotations
from typing import TYPE_CHECKING
import arcade
import arcade.clock
from pyglet.graphics import Batch

from constants import Strings

if TYPE_CHECKING:
    from ..engine import Engine


class TitleSection(arcade.Section):
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
        enabled=True,
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
        self.batch = Batch()
        self.title_text = arcade.Text(
            Strings.Title,
            self.width // 2,
            self.height - 10,
            arcade.color.RUBINE_RED,
            50,
            anchor_x="center",
            anchor_y="top",
            batch=self.batch,
        )
        self.press_text = arcade.Text(
            "",
            self.width // 2,
            175,
            arcade.color.WHITE,
            25,
            anchor_x="center",
            anchor_y="baseline",
            batch=self.batch,
        )

    def setup(self, satan_sprites: arcade.SpriteList):
        self.sprite_list = satan_sprites

    def on_draw(self):
        self.batch.draw()
        self.sprite_list.draw()

    def on_key_press(self, symbol, modifiers):
        match symbol:
            case arcade.key.ESCAPE:
                arcade.exit()
                return True
            case _:
                self.enabled = False
                self.view.menu_section.enabled = True
                return True

    def on_update(self, delta_time):
        if self.sprite_list.center[1] > self.height // 2:
            self.sprite_list.move(0, -1)
        if (arcade.clock.GLOBAL_CLOCK.time * 60) % 90 < 70:
            self.press_text.text = Strings.PressStart
        else:
            self.press_text.text = ""
