from __future__ import annotations

from typing import TYPE_CHECKING

import arcade
from pyglet.graphics import Batch

from constants import LINE_SPACING, colors

if TYPE_CHECKING:
    from engine import Engine


class MessageSection(arcade.Section):
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
        self.texts: list[arcade.Text] = []

    def on_draw(self):
        arcade.draw_lbwh_rectangle_filled(
            0, 0, self.width, self.height, colors.TranslucentBlack
        )
        self.batch.draw()

    def update_messages(self) -> None:
        self.texts.clear()
        for line in reversed(self.view.message_log.messages):
            if len(self.texts) > 0:
                y = self.texts[-1].bottom - LINE_SPACING
            else:
                y = self.height - LINE_SPACING
            self.texts.append(
                arcade.Text(
                    line.full_text,
                    5,
                    y,
                    line.color,
                    15,
                    self.width,
                    anchor_y="top",
                    multiline=True,
                    batch=self.batch,
                )
            )
