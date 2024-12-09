"""Stores, counts, and displays game messages for the player."""

from __future__ import annotations

from pygame import Surface
import pygame.freetype as freetype

class MessageLog:
    messages: list[tuple[str, int]]

    def add_message(self, message: str):
        """Adds a message to the message log.
        
        .. todo::
           - [ ] Implement adding a message"""
        pass

    def render(self, surface: Surface, font: freetype.Font):
        """Renders the message log to the supplied surface, using the supplied font.
        
        .. todo::
           - [ ] Implement rendering the message log"""
        pass