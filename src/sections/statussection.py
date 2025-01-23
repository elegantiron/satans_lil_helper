"""Status section"""

from __future__ import annotations

from typing import TYPE_CHECKING

import arcade
from pyglet.graphics import Batch

from components import Position, Stats
from constants import Strings, colors

if TYPE_CHECKING:
    from engine import Engine

LINE_SPACING = 2


class StatusSection(arcade.Section):
    """Displays the player's status"""

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
        self.batch: Batch
        self.title: arcade.Text
        self.location: arcade.Text
        self.health: arcade.Text
        self.mana: arcade.Text

    def setup(self):
        """Set up the section"""
        self.camera = arcade.Camera2D(self.rect)
        self.batch = Batch()
        self.title = arcade.Text(
            Strings.Titles.STATUS,
            self.width / 2,
            self.height - 2,
            arcade.color.WHITE,
            15,
            anchor_x="center",
            anchor_y="top",
            batch=self.batch,
        )
        self.location = arcade.Text(
            Strings.Status.LOCATION,
            5,
            self.title.bottom - LINE_SPACING,
            arcade.color.WHITE,
            15,
            anchor_y="top",
            batch=self.batch,
        )
        self.health = arcade.Text(
            Strings.Status.HEALTH,
            5,
            self.location.bottom - LINE_SPACING,
            arcade.color.WHITE,
            15,
            anchor_y="top",
            batch=self.batch,
        )
        self.mana = arcade.Text(
            Strings.Status.MANA,
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
        """Update the displayed statistics"""
        pos = self.view.player.components[Position]
        stats = self.view.player.components[Stats]
        self.location.text = f"{Strings.Status.LOCATION}{pos.x},{pos.y}"
        self.health.text = f"{Strings.Status.HEALTH}{stats.hp}/{stats.max_hp}"
        self.mana.text = f"{Strings.Status.MANA}{stats.mp}/{stats.max_mp}"
        self.set_color(stats.hp, stats.max_hp, self.health)
        self.set_color(stats.mp, stats.max_mp, self.mana)

    @staticmethod
    def set_color(current: float, maximum: float, text: arcade.Text) -> None:
        """Set an item's color"""
        if current <= maximum / 6:
            text.color = arcade.color.RED
        elif current <= maximum / 2:
            text.color = arcade.color.YELLOW
        else:
            text.color = colors.White # type: ignore
