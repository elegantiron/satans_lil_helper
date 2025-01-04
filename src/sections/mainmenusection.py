"""Main menu"""
from __future__ import annotations

from typing import TYPE_CHECKING

import arcade
from pyglet.graphics import Batch

from constants import Sections, Strings, keylists

if TYPE_CHECKING:
    from engine import Engine


class MainMenuSection(arcade.Section):
    """Main menu section"""
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
        self.sprite_list: arcade.SpriteList = None
        self.batch: Batch = None
        self.title_text: arcade.Text = None
        self.items: list[arcade.Text] = None
        self.idx: int = None

    def setup(self):
        """Set up the section"""
        self.sprite_list = self.view.satan_sprites
        self.batch = Batch()
        self.title_text = arcade.Text(
            Strings.TITLE,
            self.width // 2,
            self.height - 10,
            arcade.color.RUBINE_RED,
            50,
            anchor_x="center",
            anchor_y="top",
            batch=self.batch,
        )
        items = [
            Strings.NEW_GAME,
            Strings.BESTIARY,
            Strings.QUIT_TO_DESKTOP,
        ]
        self.items = [
            arcade.Text(
                item,
                self.width // 2,
                self.height // 2 - items.index(item) * 20,
                arcade.color.WHITE,
                20,
                anchor_x="center",
                anchor_y="center",
                batch=self.batch,
            )
            for item in items
        ]
        self.idx = 0
        self.items[0].color = arcade.color.AMERICAN_ROSE
        for item in self.items:
            item.color = item.color[0], item.color[1], item.color[2], 0

    def on_draw(self):
        self.batch.draw()
        self.sprite_list.draw()

    def on_update(self, delta_time):
        if self.sprite_list.center[1] < (self.height // 2) + 96:
            self.sprite_list.move(0, 1)
        for item in self.items:
            item.color = (
                item.color[0],
                item.color[1],
                item.color[2],
                max(
                    0,
                    int(
                        255
                        * (self.sprite_list.center[1] - (self.height // 2) - 16)
                        // 80
                    ),
                ),
            )

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
                self.view.title_section.enabled = True
                self.enabled = False
                return True
            case key if key in keylists.CONFIRMATION:
                return self.on_exit()

    def on_exit(self):
        """Handle closing the section"""
        match self.items[self.idx].text:
            case Strings.QUIT_TO_DESKTOP:
                arcade.exit()
                return True
            case Strings.NEW_GAME:
                self.view.new_world()
                self.view.gamemap_section.enabled = True
                self.view.status_section.enabled = True
                self.view.message_section.enabled = True
                self.enabled = False
                return True
            case Strings.BESTIARY:
                self.section_manager.get_section_by_name(
                    Sections.BESTIARY
                ).enabled = True
                return True
        return False

    def on_hide_section(self):
        for item in self.items:
            item.color = item.color[0], item.color[1], item.color[2], 0
