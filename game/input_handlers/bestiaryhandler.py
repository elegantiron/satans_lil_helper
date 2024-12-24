from __future__ import annotations

from typing import TypeVar

import pygame.freetype as freetype
import pygame.locals as Locals
from pygame import Surface, gfxdraw

import game.input_handlers.basehandler

from ..constants import FontDict, Sprites

Handler = TypeVar("Handler", bound="game.input_handlers.basehandler.BaseInputHandler")


class BestiaryInputHandler(game.input_handlers.basehandler.BaseInputHandler):
    def __init__(self, bestiary, parent: Handler | None = None):
        super().__init__(parent=parent)
        self.bestiary = bestiary

    def handle_key(self, key, mod, unicode, scancode):
        match key:
            case Locals.K_ESCAPE:
                if self._parent is not None:
                    return self._parent
                else:
                    raise NotImplementedError
            case _:
                return self

    def render(
        self,
        *,
        surface: Surface,
        working_surface: Surface,
        sprites: dict[Sprites, Surface],
        fonts: dict[FontDict, freetype.Font],
    ):
        font = fonts[FontDict.MainMenu]
        gfxdraw.box(surface, (40, 60, 800, 600), (0xA0, 0x20, 0x70, 0xE0))
        font.render_to(
            surface,
            (10, 10),
            text="BESTIARY",
            fgcolor=(0xFF, 0xFF, 0xFF, 0xFF),
            bgcolor=(0x00, 0x00, 0x00, 0x00),
        )
