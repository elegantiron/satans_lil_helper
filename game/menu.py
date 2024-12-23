from __future__ import annotations

from pygame import Rect, Surface, display
import pygame.freetype as freetype

from game.definitions import Color


class Menu:
    """A class to handle displaying and interacting with a text menu."""

    def __init__(
        self,
        *,
        items: list[str],
        fgcolor: Color,
        selcolor: Color,
        bgcolor: Color = Color(0, 0, 0, 0),
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

        self.fgcolor = fgcolor
        self.bgcolor = bgcolor

    def render(self, surface: Surface, font: freetype.Font):
        total_height = (
            len(self.items) * (font.get_sized_glyph_height() + self.line_spacing)
            - self.line_spacing
        )
        dest = Rect(0, 0, 0, 0)
        surf = surface.get_rect()
        dest.y = (surf.h - total_height) // 2
        for i in range(len(self.items)):
            temp = font.get_rect(text=self.items[i])
            dest.x = (self.dimensions[0] - temp.w) // 2
            font.render_to(
                surf=surface,
                dest=dest,
                text=None,
                fgcolor=self.selcolor.rgba if i == self.idx else self.fgcolor.rgba,
                bgcolor=self.bgcolor.rgba,
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
