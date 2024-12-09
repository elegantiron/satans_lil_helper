"""Various functions to help with miscellaneous activities"""

from __future__ import annotations

from pygame import Rect, Surface, display
import pygame.freetype as freetype


def calculate_fov():
    """calculates the fov for a supplied entity, area, and visibility map.
    
    .. todo::
       - [ ] Decide on FOV calculator inputs and outputs
       - [ ] Implement FOV calculation"""
    pass


def get_damage_factor(*, target_level: int, actor_level: int) -> float:
    """Calculates a damage factor.

    :param target_level: Level of the target entity
    :param actor_level: Level of the acting entity
    :return: Damage factor for the attack

    .. todo::
       - [ ] Implement calculating the damage factor"""
    pass


def get_damage(*, damage_factor: float, dice: int, sides: int) -> int:
    """Calculates the damage for an arbitrary attack.
    
    :param damage_factor: The attack's damage factor
    :param dice: The weapon's die count
    :param sides: The number of sides per die
    :return: Damage for the attack
    
    .. todo::
       - [ ] Implement damage calculator"""
    pass


def get_shade_surface(*, dims: tuple[int, int]) -> Surface:
    surface = Surface(dims)
    surface.set_alpha(0x50)
    surface.fill("black")
    return surface


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
