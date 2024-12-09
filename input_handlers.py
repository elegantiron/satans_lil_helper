"""Input handlers for different stages of the game."""

from __future__ import annotations
from typing import TypeVar

import pygame
import pygame.freetype as freetype

from actions import Action
from components import Position
from constants import KEYS, PATHS, SPRITES, STRINGS
from event_handler import EventDispatcher
from utils import Menu, get_shade_surface
from world_tools import GameWorld


class BaseEventHandler(EventDispatcher):
    """A base event handler class from which all other event handlers inherit."""

    def __init__(
        self,
        *,
        surface: pygame.Surface,  #: The surface to use for blits
        sprites: dict[
            SPRITES, pygame.Surface
        ],  #: A list of surfaces indexed by `SPRITES`
        font: freetype.Font | None = None,
    ):
        self.surface = surface
        self.font = (
            font
            if font
            else freetype.Font(
                PATHS["F25"], size=18
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

    def on_exit(self):
        pass

Handler = TypeVar("Handler", bound=BaseEventHandler)

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
    """The main input handler for the game state.

    Handles drawing everything needed to show the base game state,
    including the map, entities, fog of war, and the status screen.

    .. todo::
       - [ ] Impelment rendering the message log"""

    def __init__(self, *, surface, sprites, font=None, world=None):
        super().__init__(surface=surface, sprites=sprites, font=font, world=world)

    def ev_keydown(self, event: pygame.Event) -> Handler | None:
        """Handles a keydown event from pygame."""
        if event.key in KEYS.MOVEMENT:
            dx, dy = KEYS.MOVEMENT[event.key]
            pos = self.world.player.components.get(Position, None)
            pos.x += dx
            pos.y += dy
        elif event.key == pygame.K_ESCAPE:
            return GameMenuInputHandler(
                surface=self.surface,
                font=self.font,
                world=self.world,
                sprites=self.sprites,
            )
        else:
            super().ev_keydown(event)

    def render(self) -> None:
        super().render()
        pass


class DialogInputHandler(MainGameInputHandler):
    """Handles showing the player an NPC's dialog.

    .. todo::
       - [ ] Implement drawing the dialog
       - [ ] Implement advancing through dialog"""

    def __init__(self, *, surface, sprites, font=None):
        super().__init__(surface=surface, font=font, sprites=sprites)
        rect = self.surface.get_rect()
        self.shade = get_shade_surface((rect.w, rect.h))

    def render() -> None:
        super().render()


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
