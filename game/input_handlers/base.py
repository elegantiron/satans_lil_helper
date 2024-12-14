from __future__ import annotations

import pygame
import pygame.freetype as freetype

from ..actions import Action
from ..constants import PATHS, SPRITES

from .event_handler import EventDispatcher

class BaseInputHandler(EventDispatcher):
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
        if isinstance(state, BaseInputHandler):
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