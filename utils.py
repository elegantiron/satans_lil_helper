from __future__ import annotations

import os
from typing import TYPE_CHECKING

from pygame import Rect, Surface, display
import pygame
import pygame.freetype as freetype

from components import (
    Crit,
    Evade,
    Health,
    Inventory,
    Level,
    LightRadius,
    Magic,
    MagicDefense,
    Mana,
    Name,
    NextAction,
    PhysicalDefense,
    Renderable,
    Strength,
)
from constants import CLASSES, TAGS

if TYPE_CHECKING:
    import tcod.ecs


# TODO Decide on FOV calculator inputs and output
def calculate_fov():
    # TODO Implement FOV algorithm
    pass


def get_damage_factor(*, target_level: int, actor_level: int):
    # TODO Implement damage factor calculation
    pass


def get_damage(*, damage_factor: float, dice: int, sides: int):
    # TODO Calculate damage for an arbitrary attack
    pass


def setup_player(entity: tcod.ecs.Entity, player_class: CLASSES) -> None:
    """Adds the necessary components to an entity to make it a player."""
    entity.components[Renderable] = Renderable(
        pygame.image.load(
            os.path.join("assets", "images", "player", "player.png")
        ).convert_alpha()
    )
    entity.components[Name] = Name("Player")
    entity.components[NextAction] = NextAction(0)
    entity.components[Inventory] = Inventory(26)
    entity.components[Level] = Level(level=1, xp=0, xp_granted=0)

    entity.tags.add(TAGS.PLAYER)
    entity.tags.add(TAGS.BLOCKING)
    match player_class:
        case CLASSES.WARRIOR:
            make_warrior(entity)


def get_shade_surface(*, dims: tuple[int, int]) -> Surface:
    surface = Surface(dims)
    surface.set_alpha(0x50)
    surface.fill("black")
    return surface


def make_warrior(entity: tcod.ecs.Entity):
    """Sets up an entity to be a warrior.

    The Warrior's stats are:
    HP  : ? + ?    / level
    MP  : 5 + 1    / level
    STR : 5 + 0.5  / level
    MAG : 0 + 0.25 / level
    PDEF: 7 + 0.5  / level
    MDEF: 2 + 0.25 / level
    EVA : 0 + 0    / level
    CRIT: 0 + 0    / level
    SPD : 2 tiles/round
    LITE: 6 tiles

    EQUIPMENT SLOTS
    1-H weapon
    Shield
    Armor
    Helm
    Gauntlets
    Boots"""
    entity.components[Health] = Health(30, 10)
    entity.components[Mana] = Mana(5, 1)
    entity.components[Strength] = Strength(5, 0.5)
    entity.components[Magic] = Magic(0, 0.25)
    entity.components[PhysicalDefense] = PhysicalDefense(7, 0.5)
    entity.components[MagicDefense] = MagicDefense(2, 0.25)
    entity.components[Evade] = Evade(0, 0)
    entity.components[Crit] = Crit(0, 0)
    entity.components[LightRadius] = LightRadius(6, 0)

    entity.tags.add(TAGS.WARRIOR)
    pass


class Menu:
    """A class to handle displaying and interacting with a text menu."""

    def __init__(
        self,
        *,
        items: list[str],
        font: freetype.Font,
        fgcolor: tuple[int, int, int, int],
        selcolor: tuple[int, int, int, int] | None,
        bgcolor: tuple[int, int, int, int] = (0, 0, 0, 0),
        dimensions: tuple[int, int] | None = None,
        line_spacing: int = 5,
    ):
        self.items = items
        self.selcolor = selcolor
        self.dimensions = (
            dimensions if dimensions is not None else display.get_window_size()
        )
        self.line_spacing = line_spacing
        self.idx = 0

        self.font = font
        self.font.fgcolor = fgcolor
        self.font.bgcolor = bgcolor
        self.total_height = (
            len(self.items) * (self.font.get_sized_glyph_height() + self.line_spacing)
            - self.line_spacing
        )

    def render(self, surface: Surface):
        dest = Rect()
        surf = surface.get_rect()
        dest.y = (surf.h - self.total_height) // 2
        for i in range(len(self.items)):
            temp = self.font.get_rect(text=self.items[i])
            dest.x = (self.dimensions[0] - temp.w) // 2
            self.font.render_to(
                surf=surface,
                dest=dest,
                text=None,
                fgcolor=self.selcolor if i == self.idx else None,
            )
            dest.y = dest.y + temp.h + self.line_spacing

    def move(self, dy: int) -> None:
        self.idx += dy
        if self.idx >= 0:
            self.idx = self.idx % len(self.items)
        else:
            self.idx = len(self.items) - 1

    @property
    def confirm(self) -> int:
        return self.idx

    @property
    def item_text(self) -> str:
        return self.items[self.idx]
