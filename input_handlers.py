from __future__ import annotations
import os

import esper
import pygame
import pygame.freetype as freetype

from actions import Action
from components import Position
from constants import KEYS
from utils import EventDispatcher



class BaseEventHandler(EventDispatcher):
    def __init__(self, surface: pygame.Surface):
        self.surface = surface
        
    def handle_events(self, event: pygame.Event):
        """Handle an event and return the next active event handler."""
        state = self.dispatch(event)
        if isinstance(state, BaseEventHandler):
            return state
        assert not isinstance(state, Action), f"{self!r} can not handle actions."
        return self

    def ev_quit(self, event):
        raise SystemExit

    def on_draw(self):
        pass

    def ev_keydown(self, event):
        if event.key == pygame.K_ESCAPE:
            raise SystemExit


class MainMenuInputHandler(BaseEventHandler):
    def __init__(self, surface, font: freetype.Font | None = None):
        super().__init__(surface)
        self._idx: int = 0
        self.main_menu = [
            "New Game",
            "Exit",
        ]
        self.items: list[str] = [*self.main_menu]
        if font:
            self.font = font
        else:
            self.font = freetype.Font(
                os.path.join("assets", "fonts", "F25_Bank_Printer.ttf"), 25
            )

    @property
    def idx(self) -> int:
        return self._idx

    @idx.setter
    def idx(self, value):
        self._idx = value
        self._idx = self._idx % len(self.items)

    def on_draw(self):
        surf = self.surface.get_rect()
        dest = pygame.rect.Rect()
        dest.y = surf.h // 2
        for i in range(len(self.items)):
            temp = self.font.get_rect(
                text=self.items[i],
            )
            dest.x = (surf.w - temp.w) // 2
            self.font.render_to(
                self.surface,
                dest=dest,
                text=None,
                fgcolor=(0x00, 0x80, 0x00) if i == self.idx else (255, 255, 255, 255),
                bgcolor=(0, 0, 0, 255),
            )
            dest.y = dest.y + temp.h + 5

    def ev_keydown(self, event):
        if event.key in KEYS.MOVEMENT:
            _, dy = KEYS.MOVEMENT[event.key]
            self.idx += dy
        else:
            super().ev_keydown(event)


class MainGameInputHandler(BaseEventHandler):
    def __init__(self, player):
        self.player = player

    def ev_keydown(self, event: pygame.Event):
        print(event.key)
        if event.key in KEYS.MOVEMENT:
            dx, dy = KEYS.MOVEMENT[event.key]
            pos = esper.component_for_entity(self.player, Position)
            pos.x += dx
            pos.y += dy
        else:
            super().ev_keydown(event)
