from __future__ import annotations
from multiprocessing import Process
from pathlib import Path
from random import Random
from time import time
from typing import TYPE_CHECKING, Any, Callable, TypeVar

import numpy as np
import pygame.locals as Locals
import pygame.gfxdraw as gfxdraw


from game.components import CoolDown, Position
from game.constants import (
    CONFIRMATION_KEYS,
    MOVEMENT_KEYS,
    TILE_SIZE,
    Durations,
    FontDict,
    Sprites,
    Strings,
)
from game.exceptions import LoadGame, QuitWithoutSaving
from game.gameworld import GameWorld
from game.menu import Menu
from game.definitions import Color

if TYPE_CHECKING:
    from pygame import Surface
    import pygame.freetype as freetype
    from game.bestiary import Bestiary

Handler = TypeVar("Handler", bound="BaseInputHandler")


class BaseInputHandler:
    def __init__(self, bestiary: Bestiary, parent: Handler | None = None):
        self.bestiary = bestiary
        self._parent = parent

    def handle_key(self, key: int, mod: int, unicode, scancode: int) -> Handler:
        raise NotImplementedError

    def render(
        self,
        surface: Surface,
        sprites: dict[Sprites, Surface],
        fonts: dict[FontDict, freetype.Font],
    ):
        raise NotImplementedError

    def handle_mousemotion(self, pos, rel, buttons, touch: bool) -> Handler:
        return self

    def handle_mousebuttondown(self, pos, button, touch) -> Handler:
        return self

    def on_exit(self, *args) -> Handler:
        pass


class MainMenuInputHandler(BaseInputHandler):
    def __init__(self, bestiary):
        super().__init__(bestiary=bestiary)
        save_game = Path("./savegame.dat")
        items = []
        if save_game.exists():
            items.append(Strings.LoadGame)
        items.append(Strings.New_Game)
        items.append(Strings.Bestiary)
        items.append(Strings.QuitToDesktop)
        self.menu = Menu(
            items=items,
            fgcolor=Color(255, 255, 255, 255),
            selcolor=Color(0x00, 0x90, 0x00, 0xFF),
        )

    def render(
        self,
        *,
        surface: Surface,
        sprites: dict[Sprites, Surface],
        fonts: dict[FontDict, freetype.Font],
    ):
        self.menu.render(surface=surface, font=fonts[FontDict.MainMenu])

    def handle_key(self, key, mod, unicode, scancode):
        match key:
            case item if item in MOVEMENT_KEYS:
                if MOVEMENT_KEYS[key][1] != 0:
                    self.menu.move(MOVEMENT_KEYS[key][1])
                    return self
            case Locals.K_ESCAPE:
                raise QuitWithoutSaving
            case item if item in CONFIRMATION_KEYS:
                return self.on_exit(self.menu.item_text)
        return self

    def on_exit(self, choice: str) -> Handler:
        match choice:
            case Strings.New_Game:
                rng = Random(time())
                world = GameWorld(rng, tile_size=TILE_SIZE)
                return MainGameInputHandler(
                    world=world, rng=rng, bestiary=self.bestiary
                )
            case Strings.QuitToDesktop:
                raise QuitWithoutSaving
            case Strings.LoadGame:
                raise LoadGame
            case Strings.Bestiary:
                return BestiaryInputHandler(bestiary=self.bestiary)
            case _:
                return self


class MainGameInputHandler(BaseInputHandler):
    def __init__(
        self,
        world: GameWorld,
        rng: Random,
        bestiary: Bestiary,
        parent: Handler | None = None,
    ):
        super().__init__(bestiary=bestiary, parent=parent)
        self.world = world
        self.rng = rng

    def render(self, *, sprites, surface, fonts):
        self.world.render(
            surface=surface, sprites=sprites, font=fonts[FontDict.GameStatus]
        )

    def handle_key(self, key, mod, unicode, scancode) -> Handler:
        match key:
            case move if move in MOVEMENT_KEYS:
                dx, dy = MOVEMENT_KEYS[key]
                x, y = self.world.player.components[Position].xy
                targetx, targety = x + dx, y + dy
                if self.world.is_walkable_tile(
                    targetx, targety
                ) and self.world.player.components[Position].xy in np.ndindex(
                    self.world._current_map.tiles.shape
                ):
                    self.world.player.components[Position].x += dx
                    self.world.player.components[Position].y += dy
                    self.world.player.components[CoolDown].dur = int(
                        Durations.PlayerMovement
                    )
                    self.world.camera.set_center(
                        *self.world.player.components[Position].xy
                    )
                return self
            case Locals.K_ESCAPE:
                return GameMenuInputHandler(
                    world=self.world, rng=self.rng, bestiary=self.bestiary, parent=self
                )
            case _:
                return self


class GameMenuInputHandler(MainGameInputHandler):
    def __init__(
        self,
        world: GameWorld,
        rng: Random,
        bestiary: Bestiary,
        parent: Handler | None = None,
    ):
        super().__init__(world=world, rng=rng, bestiary=bestiary, parent=parent)
        items = [Strings.Resume, Strings.QuitWithSave, Strings.QuitNoSave]
        self.menu = Menu(
            items=items,
            fgcolor=Color(0xFF, 0xFF, 0xFF, 0xFF),
            selcolor=Color(0x00, 0x90, 0x00, 0xFF),
        )

    def render(self, *, sprites, surface, fonts):
        super().render(sprites=sprites, surface=surface, fonts=fonts)
        gfxdraw.box(surface, (0, 0, 3000, 3000), (0x00, 0x00, 0x00, 0x50))
        self.menu.render(surface=surface, font=fonts[FontDict.GameMenu])

    def handle_key(self, key, mod, unicode, scancode) -> Handler:
        match key:
            case Locals.K_ESCAPE:
                self.on_exit(Strings.Resume)
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
                return MainGameInputHandler(
                    world=self.world, rng=self.rng, bestiary=self.bestiary
                )


class BestiaryInputHandler(BaseInputHandler):
    def __init__(self, bestiary, parent: Handler | None = None):
        super().__init__(bestiary=bestiary, parent=parent)

    def handle_key(self, key, mod, unicode, scancode):
        match key:
            case Locals.K_ESCAPE:
                if self._parent is not None:
                    return self._parent
                else:
                    return MainMenuInputHandler(bestiary=self.bestiary)
            case _:
                return self

    def render(self, *, surface, sprites, fonts: dict[FontDict, freetype.Font]):
        font = fonts[FontDict.MainMenu]
        gfxdraw.box(surface, (40, 60, 800, 600), (0xA0, 0x20, 0x70, 0xE0))
        font.render_to(
            surface,
            (10, 10),
            text="BESTIARY",
            fgcolor=(0xFF, 0xFF, 0xFF, 0xFF),
            bgcolor=(0x00, 0x00, 0x00, 0x00),
        )


class LoadingHandler(BaseInputHandler):
    def __init__(
        self,
        bestiary,
        proc_func: Callable[[Any], None | Any],
        arguments: tuple[Any, ...],
        parent=None,
    ):
        super().__init__(bestiary=bestiary, parent=parent)
        self.process = Process(target=proc_func, args=arguments)
