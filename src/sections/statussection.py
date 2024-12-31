from __future__ import annotations

import arcade
from pyglet.graphics import Batch
from typing import TYPE_CHECKING
from components import Position, Stats
from constants import colors

if TYPE_CHECKING:
    from engine import Engine

LINE_SPACING = 2


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
        self.location = arcade.Text(
            "Location",
            5,
            self.title.bottom - LINE_SPACING,
            arcade.color.WHITE,
            15,
            anchor_y="top",
            batch=self.batch,
        )
        self.health = arcade.Text(
            "Health: ",
            5,
            self.location.bottom - LINE_SPACING,
            arcade.color.WHITE,
            15,
            anchor_y="top",
            batch=self.batch,
        )
        self.mana = arcade.Text(
            "Mana: ",
            self.width / 2,
            self.health.bottom,
            arcade.color.WHITE,
            15,
            anchor_y="bottom",
            batch=self.batch,
        )

    def on_draw(self):
        arcade.draw_lbwh_rectangle_filled(
            0, 0, self.width + 5, self.height, colors.TranslucentBlack
        )
        self.batch.draw()

    def update_player_stats(self):
        pos = self.view.player.components[Position]
        stats = self.view.player.components[Stats]
        self.location.text = f"Location: {pos.x},{pos.y}"
        self.health.text = f"Health: {stats.hp}/{stats.max_hp}"
        self.mana.text = f"Mana: {stats.mp}/{stats.max_mp}"
        self.set_color(stats.hp, stats.max_hp, self.health)
        self.set_color(stats.mp, stats.max_mp, self.mana)

    @staticmethod
    def set_color(current: float, max: float, text: arcade.Text) -> None:
        if current <= max / 6:
            text.color = arcade.color.RED
        elif current <= max / 2:
            text.color = arcade.color.YELLOW
        else:
            text.color = arcade.color.WHITE
