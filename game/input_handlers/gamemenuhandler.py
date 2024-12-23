from __future__ import annotations

from random import Random
from typing import TypeVar

import pygame.freetype as freetype
import pygame.locals as Locals
from pygame import Color, Surface

import game.input_handlers.basehandler
import game.input_handlers.maingamehandler

from ..bestiary import Bestiary
from ..constants import CONFIRMATION_KEYS, MOVEMENT_KEYS, FontDict, Sprites, Strings
from ..exceptions import QuitWithoutSaving
from ..gameworld import GameWorld
from ..menu import Menu

Handler = TypeVar("Handler", bound="game.input_handlers.basehandler.BaseInputHandler")


class GameMenuInputHandler(game.input_handlers.maingamehandler.MainGameInputHandler):
    def __init__(
        self,
        world: GameWorld,
        rng: Random,
        bestiary: Bestiary,
        parent: Handler,
    ):
        super().__init__(world=world, rng=rng, bestiary=bestiary, parent=parent)
        items = [Strings.Resume, Strings.QuitWithSave, Strings.QuitNoSave]
        self.menu = Menu(
            items=items,
            fgcolor=Color(0xFF, 0xFF, 0xFF, 0xFF),
            selcolor=Color(0x00, 0x90, 0x00, 0xFF),
        )

    def render(
        self,
        *,
        surface: Surface,
        working_surface: Surface,
        sprites: dict[Sprites, Surface],
        fonts: dict[FontDict, freetype.Font],
    ):
        super().render(
            sprites=sprites,
            working_surface=working_surface,
            surface=surface,
            fonts=fonts,
        )
        working_surface.fill((0, 0, 0, 255))
        working_surface.set_alpha(0x50)
        surface.blit(working_surface, (0, 0))
        self.menu.render(surface=surface, font=fonts[FontDict.GameMenu])

    def handle_key(self, key, mod, unicode, scancode) -> Handler:
        match key:
            case Locals.K_ESCAPE:
                return self.on_exit(Strings.Resume)
            case item if item in MOVEMENT_KEYS:
                self.menu.move(MOVEMENT_KEYS[key][1])
                return self
            case item if item in CONFIRMATION_KEYS:
                match self.menu.item_text:
                    case Strings.Resume:
                        if self._parent is not None:
                            return self._parent
                        else:
                            return self.on_exit(Strings.Resume)
                    case Strings.QuitNoSave:
                        raise QuitWithoutSaving
                    case Strings.QuitWithSave:
                        raise SystemExit
        return self

    def on_exit(self, selection: Strings) -> Handler:
        match selection:
            case Strings.Resume:
                raise NotImplementedError
