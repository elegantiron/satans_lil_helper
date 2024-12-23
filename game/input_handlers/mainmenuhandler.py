from __future__ import annotations
from typing import TypeVar, TYPE_CHECKING
from ..constants import Strings, FontDict, MOVEMENT_KEYS, CONFIRMATION_KEYS, Sprites
from ..exceptions import QuitWithoutSaving, LoadGame
from pathlib import Path
import pygame.locals as Locals
import game.input_handlers.basehandler
from ..menu import Menu
from pygame import Color
if TYPE_CHECKING:
    from pygame import Surface
    import pygame.freetype as freetype
    from ..bestiary import Bestiary

Handler = TypeVar("Handler", bound="game.input_handlers.basehandler.BaseInputHandler")
class MainMenuInputHandler(game.input_handlers.basehandler.BaseInputHandler):
    def __init__(self, bestiary: Bestiary):
        super().__init__(bestiary=bestiary)
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
                    return self
            case Locals.K_ESCAPE:
                raise QuitWithoutSaving
            case item if item in CONFIRMATION_KEYS:
                return self.on_exit(self.menu.item_text)
        return self

    def on_exit(self, choice: str) -> Handler:
        match choice:
            case Strings.New_Game:
                raise NotImplementedError
            case Strings.QuitToDesktop:
                raise QuitWithoutSaving
            case Strings.LoadGame:
                raise LoadGame
            case Strings.Bestiary:
                raise NotImplementedError
            case _:
                return self
