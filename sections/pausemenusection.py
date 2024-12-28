from __future__ import annotations

from typing import TYPE_CHECKING

import arcade
from pyglet.graphics import Batch

from constants import Strings, colors, keylists

if TYPE_CHECKING:
    from engine import Engine


class PauseSection(arcade.Section):
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
        local_mouse_coordinates=True,
        enabled=False,
        modal=True,
        draw_order=3,
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
        self.camera = arcade.Camera2D(self.rect)
        self.title = arcade.Text(
            "PAUSE",
            self.width / 2,
            self.height - 2,
            arcade.color.WHITE,
            15,
            anchor_x="center",
            anchor_y="top",
            batch=self.batch,
        )
        items = [
            Strings.Resume,
            Strings.Bestiary,
            Strings.SaveAndQuit,
            Strings.QuitNoSave,
        ]
        self.items = [
            arcade.Text(
                item,
                self.width // 2,
                self.height // 2 - items.index(item) * 30,
                arcade.color.WHITE,
                15,
                anchor_x="center",
                anchor_y="center",
                batch=self.batch,
            )
            for item in items
        ]
        self.idx = 0
        self.items[0].color = arcade.color.AMERICAN_ROSE

    def on_draw(self):
        arcade.draw_lbwh_rectangle_filled(
            0, 0, self.width, self.height, colors.TranslucentBlack
        )
        arcade.draw_lbwh_rectangle_outline(
            0, 0, self.width, self.height, arcade.color.WHITE, 2
        )
        self.batch.draw()

    def on_key_press(self, symbol, modifiers):
        match symbol:
            case key if key in keylists.MOVEMENT and keylists.MOVEMENT[key][1] != 0:
                self.items[self.idx].color = arcade.color.WHITE
                self.idx -= keylists.MOVEMENT[key][1]
                if self.idx < 0:
                    self.idx = len(self.items) - 1
                else:
                    self.idx = self.idx % len(self.items)
                self.items[self.idx].color = arcade.color.AMERICAN_ROSE
                return True
            case arcade.key.ESCAPE:
                self.enabled = False
                return True
            case key if key in keylists.CONFIRMATION:
                return self.on_exit()

    def on_exit(self):
        match self.items[self.idx].text:
            case Strings.Resume:
                self.enabled = False
                return True
            case Strings.QuitNoSave:
                arcade.exit()
                return True
            case Strings.SaveAndQuit:
                arcade.exit()
                return True
            case Strings.Bestiary:
                self.view.bestiary_section.enabled = True
                self.enabled = False
                return True
        return False

    def on_mouse_motion(self, x, y, dx, dy):
        for item in self.items:
            if int(x) in range(int(item.left), int(item.right)) and int(y) in range(
                int(item.bottom), int(item.top)
            ):
                self.items[self.idx].color = arcade.color.WHITE
                item.color = arcade.color.AMERICAN_ROSE
                self.idx = self.items.index(item)

    def on_mouse_press(self, x, y, button, modifiers):
        match button:
            case arcade.MOUSE_BUTTON_LEFT:
                self.on_exit()

