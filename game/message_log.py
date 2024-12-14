"""Stores, counts, and displays game messages for the player.

.. todo::
    - [ ] Implement adding a message
    - [ ] Implement rendering the message log
"""

from __future__ import annotations

from pygame import Surface
import pygame.freetype as freetype

class MessageLog:
    messages: list[tuple[str, int]]

    def add_message(self, message: str):
        pass

    def render(self, surface: Surface, font: freetype.Font):
        pass