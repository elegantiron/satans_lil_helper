from __future__ import annotations
from typing import TypeVar

import pygame.freetype as freetype

from constants import KEYS, STRINGS
from input_handlers import BaseInputHandler, MainGameInputHandler
from tools import GameWorld, Menu

Handler = TypeVar("Handler", bound=BaseInputHandler)

class MainMenuInputHandler(BaseInputHandler):
    def __init__(self, *, surface, sprites, font: freetype.Font | None = None):
        super().__init__(surface=surface, font=font, sprites=sprites)
        self._idx: int = 0
        self.menu = Menu(
            items=[STRINGS.NEW_GAME, STRINGS.EXIT],
            font=self.font,
            fgcolor=(255, 255, 255, 255),
            selcolor=(0x00, 0x90, 0x00, 0xFF),
        )

    def render(self):
        self.surface.fill("black")
        self.menu.render(self.surface)

    def ev_keydown(self, event) -> Handler | None:
        if event.key in KEYS.MOVEMENT:
            _, dy = KEYS.MOVEMENT[event.key]
            self.menu.move(dy)
        elif event.key in KEYS.CONFIRMATION:
            match self.menu.item_text:
                case STRINGS.NEW_GAME:
                    new_world = GameWorld()
                    return MainGameInputHandler(
                        surface=self.surface,
                        sprites=self.sprites,
                        font=self.font,
                        world=new_world,
                    )
                case STRINGS.EXIT:
                    raise SystemExit
        else:
            super().ev_keydown(event)
