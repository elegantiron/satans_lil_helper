from __future__ import annotations

from pathlib import Path
from typing import TYPE_CHECKING, TypeVar

import pygame.locals as Locals
from pygame import Color

from . import basehandler

from ..constants import (
    CONFIRMATION_KEYS,
    MOVEMENT_KEYS,
    FontDict,
    HandlerActions,
    Sprites,
    Strings,
)
from ..exceptions import QuitWithoutSaving
from ..menu import Menu

if TYPE_CHECKING:
    import pygame.freetype as freetype
    from pygame import Surface

Handler = TypeVar("Handler", bound="basehandler.BaseInputHandler")


class MainMenuInputHandler(basehandler.BaseInputHandler):
    def __init__(self):
        super().__init__()
        save_game = Path("./savegame.dat")
        items = []
        if save_game.exists():
            items.append(Strings.LoadGame)
        items.append(Strings.New_Game)
        items.append(Strings.Bestiary)
        items.append(Strings.QuitToDesktop)
        self.menu = Menu(
            items=items,
            fgcolor=Color(255, 255, 255, 255),
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
        self.menu.render(surface=surface, font=fonts[FontDict.MainMenu])
        dest = fonts[FontDict.TitleText].get_rect(text=Strings.Title)
        fonts[FontDict.TitleText].render_to(
            surf=surface, dest=((surface.get_width() - dest.w) // 2, 15), text=None
        )

    def handle_key(self, key, mod, unicode, scancode):
        match key:
            case item if item in MOVEMENT_KEYS:
                if MOVEMENT_KEYS[key][1] != 0:
                    self.menu.move(MOVEMENT_KEYS[key][1])
                    return HandlerActions.Noop
            case Locals.K_ESCAPE:
                raise QuitWithoutSaving
            case item if item in CONFIRMATION_KEYS:
                return self.on_exit(self.menu.item_text)
        return self

    def on_exit(self, choice: str) -> Handler:
        match choice:
            case Strings.New_Game:
                return HandlerActions.NewGame
            case Strings.QuitToDesktop:
                raise QuitWithoutSaving
            case Strings.LoadGame:
                return HandlerActions.LoadGame
            case Strings.Bestiary:
                return HandlerActions.ShowBestiary
            case _:
                return self
