from __future__ import annotations

from typing import TypeVar

import pygame.freetype as freetype
import pygame.locals as Locals
from pygame import Color, Surface
from . import basehandler


from ..constants import CONFIRMATION_KEYS, MOVEMENT_KEYS, FontDict, Sprites, Strings, HandlerActions
from ..exceptions import QuitWithoutSaving
from ..menu import Menu

Handler = TypeVar("Handler", bound="basehandler.BaseInputHandler")


class GameMenuInputHandler(basehandler.BaseInputHandler):
    def __init__(
        self,
        parent: Handler,
    ):
        super().__init__(parent=parent)
        items = [Strings.Resume, Strings.QuitWithSave, Strings.QuitNoSave]
        self.menu = Menu(
            items=items,
            fgcolor=Color(0xFF, 0xFF, 0xFF, 0xFF),
            selcolor=Color(0x00, 0x90, 0x00, 0xFF),
        )

    def render(
        self,
        *,
        surface: Surface,
        working_surface: Surface,
        sprites: dict[Sprites, Surface],
        fonts: dict[FontDict, freetype.Font],
    ):
        super().render(
            sprites=sprites,
            working_surface=working_surface,
            surface=surface,
            fonts=fonts,
        )
        working_surface.fill((0, 0, 0, 255))
        working_surface.set_alpha(0x50)
        surface.blit(working_surface, (0, 0))
        self.menu.render(surface=surface, font=fonts[FontDict.GameMenu])

    def handle_key(self, key, mod, unicode, scancode) -> Handler:
        match key:
            case Locals.K_ESCAPE:
                return HandlerActions.ShowParent
            case item if item in MOVEMENT_KEYS:
                self.menu.move(MOVEMENT_KEYS[key][1])
                return HandlerActions.Noop
            case item if item in CONFIRMATION_KEYS:
                match self.menu.item_text:
                    case Strings.Resume:
                        return HandlerActions.ShowParent
                    case Strings.QuitNoSave:
                        raise QuitWithoutSaving
                    case Strings.QuitWithSave:
                        raise SystemExit
        return self

