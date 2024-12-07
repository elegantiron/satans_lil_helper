from __future__ import annotations
import os

import pygame
import pygame.freetype as freetype

from actions import Action
from components import Position
from constants import KEYS, STRINGS
from event_handler import EventDispatcher
from utils import Menu, get_shade_surface
from world_tools import GameWorld


class BaseEventHandler(EventDispatcher):
    def __init__(
        self,
        *,
        surface: pygame.Surface,
        sprites: list[pygame.Surface],
        font: freetype.Font | None = None,
    ):
        self.surface = surface
        self.font = (
            font
            if font
            else freetype.Font(
                os.path.join("assets", "fonts", "F25_Bank_Printer.ttf"), size=18
            )
        )
        self.font.fgcolor = (255, 255, 255, 255)
        self.sprites = sprites

    def handle_events(self, event: pygame.Event):
        """Handle an event and return the next active event handler."""
        state = self.dispatch(event)
        if isinstance(state, BaseEventHandler):
            return state
        assert not isinstance(state, Action), f"{self!r} can not handle actions."
        return self

    def ev_quit(self, event):
        raise SystemExit

    def render(self):
        raise NotImplementedError

    def ev_keydown(self, event):
        if event.key == pygame.K_ESCAPE:
            raise SystemExit


class MainMenuInputHandler(BaseEventHandler):
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

    def ev_keydown(self, event):
        if event.key in KEYS.MOVEMENT:
            _, dy = KEYS.MOVEMENT[event.key]
            self.menu.move(dy)
        elif event.key in KEYS.CONFIRMATION:
            match self.menu.item_text:
                case STRINGS.NEW_GAME:
                    pass
                case STRINGS.EXIT:
                    raise SystemExit
        else:
            super().ev_keydown(event)


class InGameInputHandler(BaseEventHandler):
    def __init__(self, *, surface, sprites, font=None, world: GameWorld = None):
        super().__init__(surface=surface, sprites=sprites, font=font)
        if world is not None:
            self.world = world
        else:
            self.world = GameWorld()

    def render(self):
        self.world.render(self.surface, self.sprites)


class MainGameInputHandler(InGameInputHandler):
    def __init__(self, *, surface, sprites, font=None, world=None):
        super().__init__(surface=surface, sprites=sprites, font=font, world=world)

    def ev_keydown(self, event: pygame.Event):
        if event.key in KEYS.MOVEMENT:
            dx, dy = KEYS.MOVEMENT[event.key]
            pos = self.world.player.components.get(Position, None)
            pos.x += dx
            pos.y += dy
        elif event.key == pygame.K_ESCAPE:
            return GameMenuInputHandler(self.surface, self.font, self.world)
        else:
            super().ev_keydown(event)

    def render(self) -> None:
        super().render()
        # TODO Implement drawing the message log
        pass


class DialogInputHandler(MainGameInputHandler):
    def __init__(self, *, surface, sprites, font=None):
        super().__init__(surface=surface, font=font, sprites=sprites)
        rect = self.surface.get_rect()
        self.shade = get_shade_surface((rect.w, rect.h))

    def render():
        super().render()
        # TODO Implement drawing the dialog


class GameMenuInputHandler(MainGameInputHandler):
    def __init__(self, *, surface, sprites, font=None):
        super().__init__(surface=surface, sprites=sprites, font=font)
        rect = self.surface.get_rect()
        self.shade = get_shade_surface((rect.w, rect.h))

    def render(self):
        super().render()
        self.surface.blit(self.shade)

    def ev_keydown(self, event):
        if event.key == pygame.K_ESCAPE:
            return MainGameInputHandler(self.surface, self.font, self.world)
