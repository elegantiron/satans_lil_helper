from __future__ import annotations

import abc
from typing import TYPE_CHECKING, TypeVar

if TYPE_CHECKING:
    import pygame.freetype as freetype
    from pygame import Surface

    from ..bestiary import Bestiary
    from ..constants import FontDict, Sprites

Handler = TypeVar("Handler", bound="BaseInputHandler")


class BaseInputHandler(metaclass=abc.ABCMeta):
    def __init__(self, bestiary: Bestiary, parent: Handler | None = None):
        self.bestiary = bestiary
        self._parent = parent

    @abc.abstractmethod
    def handle_key(self, key: int, mod: int, unicode, scancode: int) -> Handler:
        return self

    @abc.abstractmethod
    def render(
        self,
        *,
        surface: Surface,
        working_surface: Surface,
        sprites: dict[Sprites, Surface],
        fonts: dict[FontDict, freetype.Font],
    ):
        pass

    def handle_mousemotion(self, pos, rel, buttons, touch: bool) -> Handler:
        return self

    def handle_mousebuttondown(self, pos, button, touch) -> Handler:
        return self

    def on_exit(self, *args) -> Handler:
        pass

    def _receive_process_data(self, data):
        raise NotImplementedError
