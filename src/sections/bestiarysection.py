"""Bestiary display section"""

from __future__ import annotations

from typing import TYPE_CHECKING

import arcade
from pyglet.graphics import Batch

from constants import Strings

if TYPE_CHECKING:
    from engine import Engine


class BestiarySection(arcade.Section):
    """Handles drawing and inputs for the bestiary"""

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
        prevent_dispatch  = None,
        prevent_dispatch_view  = None,
        local_mouse_coordinates = False,
        enabled = False,
        modal = True,
        draw_order = 5,
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
        self.batch: Batch
        self.title: arcade.Text
        self.view: Engine

    def setup(self) -> None:
        """Set up the section"""
        self.batch = Batch()
        self.title = arcade.Text(
            Strings.Titles.BESTIARY,
            self.width // 2,
            self.height - 10,
            arcade.color.RUBINE_RED,
            50,
            anchor_x="center",
            anchor_y="top",
            batch=self.batch,
        )

    def on_draw(self) -> None:
        arcade.draw_lbwh_rectangle_filled(
            0, 0, self.width, self.height, arcade.color.BLACK
        )
        self.batch.draw()

    def on_key_press(self, symbol: int, modifiers: int) -> None:
        match symbol:
            case arcade.key.ESCAPE:
                self.enabled = False
                if self.view.gamemap_section.enabled:
                    self.view.pause_section.enabled = True
