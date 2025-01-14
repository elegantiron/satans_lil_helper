from __future__ import annotations

from typing import TYPE_CHECKING

import arcade
from pyglet.graphics import Batch

from components import Skills
from constants import Strings, abilities, colors
from exceptions import MissingComponent

if TYPE_CHECKING:
    from engine import Engine


"LEVEL UP"


class LevelupSection(arcade.Section):
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
        self.batch: Batch
        self.title: arcade.Text
        self.skill_options: list[abilities.Skill]
        self.skill_names: list[arcade.Text]
        self.skill_descriptions: list[arcade.Text]
        self.sprite_list: arcade.SpriteList

    def setup(self):
        self.sprite_list = arcade.SpriteList()
        self.camera = arcade.Camera2D(self.rect)
        self.batch = Batch()
        self.title = arcade.Text(
            Strings.Titles.LEVEL_UP,
            self.width / 2,
            self.height - 2,
            colors.White,
            15,
            anchor_x="center",
            anchor_y="top",
            batch=self.batch,
        )

    def pick_skills(
        self,
    ) -> list[abilities.Abilities, abilities.Abilities, abilities.Abilities]:
        player_skills = self.view.player.components.get(Skills, None)
        if player_skills is None:
            raise MissingComponent
        possibilities = [
            item
            for item in abilities.SkillList
            if (
                item.skill_id not in player_skills.onetime
                and (item.prereqs is None or item.prereqs in player_skills.onetime)
            )
        ]
        self.skill_options = self.view.rng.sample(possibilities, 3)

    def on_draw(self):
        arcade.draw_lbwh_rectangle_filled(
            0, 0, self.width, self.height, colors.TranslucentBlack
        )
        arcade.draw_lbwh_rectangle_outline(
            0, 0, self.width, self.height, colors.White, 2
        )
        self.batch.draw()
        self.sprite_list.draw()

    def on_show_section(self):
        self.pick_skills()

    def on_hide_section(self):
        self.sprite_list.clear()