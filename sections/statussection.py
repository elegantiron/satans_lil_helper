from __future__ import annotations

import arcade
from pyglet.graphics import Batch
from typing import TYPE_CHECKING
from components import Position

if TYPE_CHECKING:
    from engine import Engine

class StatusSection(arcade.Section):
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
        self.batch = Batch()
        self.title = arcade.Text(
            "STATUS",
            self.width / 2,
            self.height - 2,
            arcade.color.WHITE,
            15,
            anchor_x="center",
            anchor_y="top",
            batch=self.batch,
        )
        self.player_location = arcade.Text(
            "Location",
            5,
            self.title.bottom + 5,
            arcade.color.WHITE,
            15,
            anchor_y="top",
            batch=self.batch,
        )

    def on_draw(self):
        arcade.draw_lbwh_rectangle_filled(
            0, 0, self.width + 5, self.height, (0, 0, 0, 0x70)
        )
        self.batch.draw()

    def update_player_stats(self):
        pos = self.view.player.components[Position]
        self.player_location.text = f"Location: {pos.x},{pos.y}"