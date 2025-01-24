from __future__ import annotations

from typing import TYPE_CHECKING

import arcade
from pyglet.graphics import Batch

import abilities
from components import Skills, Stats
from constants import Strings, colors
from exceptions import MissingComponent

if TYPE_CHECKING:
    from engine import Engine


class CharacterSection(arcade.Section):
    batch: Batch
    stats: arcade.Text
    title: arcade.Text

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
        self.view: Engine
        self.camera: arcade.Camera2D

    def setup(self):
        self.camera = arcade.Camera2D(self.rect)
        self.batch = Batch()
        self.title = arcade.Text(
            Strings.Titles.CHARACTER_SHEET.title(),
            self.width // 2,
            self.height - 2,
            colors.White,
            20,
            anchor_x="center",
            anchor_y="top",
            batch=self.batch,
        )
        self.stats = arcade.Text(
            "Stats",
            5,
            self.title.bottom - 5,
            colors.White,
            15,
            anchor_x="left",
            anchor_y="top",
            batch=self.batch,
        )
        self.healthmana = arcade.Text(
            Strings.Status.HEALTH,
            5,
            self.stats.bottom,
            colors.White,
            15,
            anchor_y="top",
            batch=self.batch,
        )
        self.abilityscores = arcade.Text(
            "Placeholder",  # This is exclusively so that the sizes we use later are calculated
            5,
            self.healthmana.bottom,
            colors.White,
            15,
            int(self.width / 4),
            anchor_y="top",
            multiline=True,
            batch=self.batch,
        )
        self.equipment = arcade.Text(
            "Placeholder",
            self.width / 4,
            self.healthmana.bottom,
            colors.White,
            15,
            int(self.width / 4),
            anchor_y="top",
            multiline=True,
            batch=self.batch,
        )
        self.skills = arcade.Text(
            "Placeholder",
            self.width / 2,
            self.healthmana.bottom,
            colors.White,
            15,
            int(self.width / 4),
            anchor_y="top",
            multiline=True,
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
            case arcade.key.ESCAPE | arcade.key.C:
                self.enabled = False

    def on_show_section(self):
        stats = self.view.player.components.get(Stats)
        if stats is None:
            raise MissingComponent
        self.healthmana.text = (
            f"{Strings.Status.HEALTH.capitalize()}: {stats.hp}/{stats.max_hp}\t"
            f"{Strings.Status.MANA.capitalize()}: {stats.mp}/{stats.max_mp}"
        )
        self.abilityscores.text = (
            "\n"
            f"{Strings.Status.ABILITIES.upper()}      \n"
            f"{Strings.Status.STRENGTH.capitalize()}:\t\t{stats.strength}\n"
            f"{Strings.Status.MAGIC.capitalize()}:\t\t{stats.magic}\n"
            f"{Strings.Status.EVASION.capitalize()}:\t\t{stats.evasion}\n"
            f"{Strings.Status.CRIT.capitalize()}:\t\t\t{stats.crit}\n"
            f"{Strings.Status.LIGHT.capitalize()}:\t\t{stats.light}\n"
            f"{Strings.Status.VISION.capitalize()}:\t{stats.sight}\n"
        )
        self.equipment.text = (
            "\n"
            f"{Strings.Status.EQUIPMENT.upper()}\n"
            f"{Strings.GearSlots.WEAPON.capitalize()}:\tSword\n"
            f"{Strings.GearSlots.HEAD.capitalize()}:\t\tHelmet\n"
            f"{Strings.GearSlots.BODY.capitalize()}:\tArmor\n"
            f"{Strings.GearSlots.HANDS.capitalize()}:\tHands\n"
            f"{Strings.GearSlots.FEET.capitalize()}:\tBoots\n"
        )
        self.skills.text = f"\n{Strings.Status.SKILLS.upper()}\n"
        for skill in self.view.player.components[Skills].repeatable:
            for req in abilities.SkillList:
                if (
                    req.skill_id == skill
                    and self.view.player.components[Skills].repeatable[skill] > 0
                ):
                    self.skills.text = f"{self.skills.text}{req.name.title()}\n"
        for skill in self.view.player.components[Skills].onetime:
            for req in abilities.SkillList:
                if req.skill_id == skill:
                    self.skills.text = f"{self.skills.text}{req.name.title()}\n"