from __future__ import annotations
from typing import TypeVar

import pygame

from constants import KEYS, STRINGS
from input_handlers import BaseInputHandler, MainGameInputHandler, MainMenuInputHandler
from tools import get_shade_surface, Menu

Handler = TypeVar("Handler", bound="BaseInputHandler")


class GameMenuInputHandler(MainGameInputHandler):
    def __init__(self, *, surface, sprites, font=None, world):
        super().__init__(surface=surface, sprites=sprites, font=font, world=world)
        rect = self.surface.get_rect()
        self.shade = get_shade_surface(dims=(rect.w, rect.h))
        self.menu = Menu(
            items=[STRINGS.CONTINUE, STRINGS.EXIT_TO_MENU, STRINGS.EXIT_TO_DESKTOP],
            font=self.font,
            fgcolor=(0xFF, 0xFF, 0xFF, 0xFF),
            selcolor=(0x00, 0x80, 0x00, 0xFF),
        )

    def render(self):
        super().render()
        self.surface.blit(self.shade)
        self.menu.render(surface=self.surface)

    def ev_keydown(self, event):
        if event.key in KEYS.MOVEMENT:
            _, dy = KEYS.MOVEMENT[event.key]
            self.menu.move(dy)
        elif event.key in KEYS.CONFIRMATION:
            match self.menu.item_text:
                case STRINGS.CONTINUE:
                    return self.on_exit()
                case STRINGS.EXIT_TO_MENU:
                    return MainMenuInputHandler(
                        surface=self.surface, sprites=self.sprites, font=self.font
                    )
                case STRINGS.EXIT_TO_DESKTOP:
                    raise SystemExit
        elif event.key == pygame.K_ESCAPE:
            return self.on_exit()
        else:
            super().ev_keydown(event)

    def on_exit(self):
        return MainGameInputHandler(
            surface=self.surface,
            font=self.font,
            world=self.world,
            sprites=self.sprites,
        )
