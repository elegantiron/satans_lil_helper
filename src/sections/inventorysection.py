"""Inventory section"""

from __future__ import annotations

import arcade
from pyglet.graphics import Batch

from constants import Strings, colors


class InventorySection(arcade.Section):
    """Handles drawing and inputs for the inventory"""

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
        modal=True,
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
        self.camera: arcade.Camera2D
        self.batch: Batch
        self.title: arcade.Text

    def setup(self):
        """Set up the section"""
        self.camera = arcade.Camera2D(self.rect)
        self.batch = Batch()
        self.title = arcade.Text(
            Strings.INVENTORY_TITLE,
            self.width / 2,
            self.height - 3,
            colors.White,
            15,
            anchor_x="center",
            anchor_y="top",
            batch=self.batch,
        )

    def on_draw(self):
        arcade.draw_lbwh_rectangle_filled(
            0, 0, self.width, self.height, colors.TranslucentBlack
        )
        arcade.draw_lbwh_rectangle_outline(
            0, 0, self.width, self.height, colors.White, 2
        )
        self.batch.draw()

    def on_key_press(self, symbol, modifiers):
        match symbol:
            case arcade.key.ESCAPE:
                self.enabled = False
