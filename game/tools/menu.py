from __future__ import annotations

from pygame import Rect, Surface, display
import pygame.freetype as freetype

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
