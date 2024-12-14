from __future__ import annotations
from typing import TypeVar

import pygame

from constants import KEYS
from components import Position
from input_handlers import BaseInputHandler, GameMenuInputHandler, InGameInputHandler

Handler = TypeVar("Handler", bound="BaseInputHandler")

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
