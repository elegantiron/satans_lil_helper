from __future__ import annotations

from pygame import Surface
import pygame.freetype as freetype

class MessageLog:
    messages: list[tuple[str, int]]

    def add_message(self, message: str):
        # TODO Implement adding messages to the message log.
        pass

    def render(self, surface: Surface, font: freetype.Font):
        # TODO Implement rendering the message log.
        pass