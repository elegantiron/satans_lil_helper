"""Shows information from the inspector"""

from __future__ import annotations

from typing import TYPE_CHECKING

import arcade
import numpy as np
from pyglet.graphics import Batch

from components import Name, Position
from constants import LINE_SPACING, Tile, colors

if TYPE_CHECKING:
    from engine import Engine


class InspectorSection(arcade.Section):
    """Draws and handles inputs for the inspector"""

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
        self.camera: arcade.Camera2D
        self.batch: Batch
        self.title: arcade.Text
        self.location: arcade.Text
        self.description: arcade.Text

    def setup(self):
        """Set up the section"""
        self.camera = arcade.Camera2D(self.rect)
        self.batch = Batch()
        self.title = arcade.Text(
            "INSPECTOR",
            self.width / 2,
            self.height - 2,
            font_size=15,
            anchor_x="center",
            anchor_y="top",
            batch=self.batch,
        )
        self.location = arcade.Text(
            "Location: ",
            5,
            self.title.bottom - LINE_SPACING,
            font_size=15,
            anchor_y="top",
            batch=self.batch,
        )
        self.description = arcade.Text(
            "",
            5,
            self.location.bottom - LINE_SPACING,
            font_size=15,
            anchor_y="top",
            batch=self.batch,
            width=self.width - 10,
            multiline=True,
        )

    def on_draw(self):
        arcade.draw_lbwh_rectangle_filled(
            0, 0, self.width, self.height, colors.TranslucentBlack
        )
        self.batch.draw()

    def update(self):
        """Update display based on what the inspector found"""
        highlight = self.view.gamemap_section.highlight
        self.location.text = f"Location: {int(highlight[0])},{int(highlight[1])}"
        description = ""
        if highlight in np.ndindex(self.view.world.map.tiles.shape):
            if self.view.world.map.tiles[Tile.VISIBLE][highlight]:
                description = f"{description}You see"
                found = False
                for ent in self.view.world.map.registry.Q.all_of(components=[Position]):
                    pos = ent.components[Position]
                    if pos.x == highlight[0] and pos.y == highlight[1]:
                        name = ent.components.get(Name, "a mysterious stranger")
                        if name.name == "you":
                            description = f"{description} yourself"
                        else:
                            description = f"{description} {name.name}"
                        found = True
                if not found:
                    description = f"{description} nothing"
                description = f"{description}. "
            if self.view.world.map.tiles[Tile.EXPLORED][highlight]:
                description = f"{description}The tile is"
                if not self.view.world.map.tiles[Tile.WALKABLE][highlight]:
                    description = f"{description}n't"
                description = f"{description} walkable. You can"
                if not self.view.world.map.tiles[Tile.TRANSPARENT][highlight]:
                    description = f"{description}'t"
                description = f"{description} see through the tile."
            if (
                not self.view.world.map.tiles[Tile.EXPLORED][highlight]
                and not self.view.world.map.tiles[Tile.VISIBLE][highlight]
            ):
                description = "You know not what this tile contains."
        else:
            description = "Your view is consumed by The Void."
        self.description.text = description
