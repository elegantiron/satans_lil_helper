from __future__ import annotations

from typing import TypeVar

import pygame.locals as Locals
from pygame import Rect, draw

import game.colors as colors

from ..constants import (
    CONFIRMATION_KEYS,
    MOVEMENT_KEYS,
    FontDict,
    HandlerActions,
    ProfessionDescriptions,
    Strings,
)
from ..exceptions import QuitWithoutSaving
from . import basehandler

Handler = TypeVar("Handler", bound="basehandler.BaseInputHandler")


class SelectClassInputHandler(basehandler.BaseInputHandler):
    def __init__(self, parent: Handler | None = None):
        super().__init__(parent)
        self.idx = 0

    def render(self, *, surface, working_surface, sprites, fonts):
        size1 = fonts[FontDict.TitleText].get_rect(Strings.ChoosePlayerClass)
        dest = Rect((surface.get_width() - size1.w) // 2, 20, 0, 0)
        fonts[FontDict.TitleText].render_to(surf=surface, dest=dest, text=None)
        text = ProfessionDescriptions[self.idx][1]
        size2 = fonts[FontDict.ClassName].get_rect(text)
        dest = Rect((surface.get_width() - size2.w) // 2, size1.bottom + 20, 0, 0)
        fonts[FontDict.ClassName].render_to(surf=surface, dest=dest, text=None)
        dest = Rect(
            (surface.get_width() - 150) // 2,
            (surface.get_height() - 300) // 2,
            150,
            300,
        )
        draw.rect(surface, colors.Impossible, dest)
        dest.bottom += 25
        for text in ProfessionDescriptions[self.idx][2]:
            size1 = fonts[FontDict.GameStatus].get_rect(text)
            dest = Rect(
                (surface.get_width() - size1.w) // 2, dest.bottom + 5, size1.w, size1.h
            )
            fonts[FontDict.GameStatus].render_to(surface, dest, text=None)

    def handle_key(self, key, mod, unicode, scancode):
        match key:
            case Locals.K_ESCAPE:
                raise QuitWithoutSaving
            case key if key in MOVEMENT_KEYS and MOVEMENT_KEYS[key][0] != 0:
                self.idx += MOVEMENT_KEYS[key][0]
                if self.idx < 0:
                    self.idx = len(ProfessionDescriptions) - 1
                self.idx = self.idx % len(ProfessionDescriptions)
            case key if key in CONFIRMATION_KEYS:
                return HandlerActions.NewGame
            case _:
                return HandlerActions.Noop
        return self
