from __future__ import annotations

import abc
import lzma
from multiprocessing import Process, Queue
from pathlib import Path
from random import Random
from time import time
from typing import TYPE_CHECKING, TypeVar

import dill
import pygame.gfxdraw as gfxdraw
import pygame.locals as Locals
from pygame import Rect
from pygame import display as pygdisp

import game.colors as colors
from game import loaders
from game.actions import BumpAction
from game.components import Position
from game.constants import (
    CONFIRMATION_KEYS,
    MOVEMENT_KEYS,
    TILE_SIZE,
    FontDict,
    Sprites,
    Strings,
)
from game.definitions import Color
from game.exceptions import AsyncException, LoadGame, PathBlocked, QuitWithoutSaving
from game.gameworld import GameWorld
from game.menu import Menu

if TYPE_CHECKING:
    import pygame.freetype as freetype
    from pygame import Surface

    from game.bestiary import Bestiary

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


class LoadingHandler(BaseInputHandler, metaclass=abc.ABCMeta):
    """Base class for loading/processing data in a separate process.

    Allows for performing longer operations without causing the operating system
    to think that we have frozen."""

    def __init__(
        self,
        bestiary: Bestiary,
        rng: Random,
        process: Process,
        queue: Queue,
        parent: Handler,
    ):
        super().__init__(bestiary, parent)
        self.rng = rng
        self.process = process
        self.queue = queue
        
    def render(
        self,
        *,
        surface: Surface,
        working_surface: Surface,
        sprites: dict[Sprites, Surface],
        fonts: dict[FontDict, freetype.Font],
    ):
        if self.process.exitcode is None:
            size = fonts[FontDict.MainMenu].get_rect(
                text=f"Loading{['.' for _ in range((time() * 4) % 4)]}"
            )
            dest = Rect((surface.get_width() - size.w) // 2, 35, 0, 0)
            fonts[FontDict.MainMenu].render_to(surface, dest, text=None)
        elif self.process.exitcode == 0:
            size = fonts[FontDict.MainMenu].get_rect(
                text="Press any key to cotinue"
            )
            dest = Rect((surface.get_width() - size.w) // 2, 35, 0, 0)
            fonts[FontDict.MainMenu].render_to(
                surface, dest, "Press any key to continue"
            )
        else:
            size = fonts[FontDict.MainMenu].get_rect(
                text=f"Error encountered! Exit code {self.process.exitcode}"
            )
            dest = Rect((surface.get_width() - size.w) // 2, 35, 0, 0)
            fonts[FontDict.MainMenu].render_to(
                surface, dest, f"Error encountered! Exit code {self.process.exitcode}"
            )
    
    def handle_key(self, key, mod, unicode, scancode):
        if self.process.exitcode is None:
            return self
        elif self.process.exitcode == 0:
            data = self.queue.get()
            self._parent._receive_process_data(data)
            return self._parent
        else:
            return self


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
        working_surface: Surface,
        sprites: dict[Sprites, Surface],
        fonts: dict[FontDict, freetype.Font],
    ):
        self.menu.render(surface=surface, font=fonts[FontDict.MainMenu])
        dest = fonts[FontDict.TitleText].get_rect(text=Strings.Title)
        fonts[FontDict.TitleText].render_to(
            surf=surface, dest=((surface.get_width() - dest.w) // 2, 15), text=None
        )

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
                return SelectPlayerClassHandler(bestiary=self.bestiary)
            case Strings.QuitToDesktop:
                raise QuitWithoutSaving
            case Strings.LoadGame:
                raise LoadGame
            case Strings.Bestiary:
                return BestiaryInputHandler(bestiary=self.bestiary)
            case _:
                return self


class SelectPlayerClassHandler(BaseInputHandler):
    def __init__(self, bestiary: Bestiary, parent: Handler | None = None):
        super().__init__(bestiary, parent)

    def render(self, *, surface, working_surface, sprites, fonts):
        size = fonts[FontDict.ChooseClass].get_rect(Strings.ChoosePlayerClass)
        dest = Rect((surface.get_width() - size.w) // 2, 20, 0, 0)
        fonts[FontDict.ChooseClass].render_to(surf=surface, dest=dest, text=None)

    def handle_key(self, key, mod, unicode, scancode):
        match key:
            case Locals.K_ESCAPE:
                raise QuitWithoutSaving
            case _:
                rng = Random(time())
                world = GameWorld(
            rng=rng, tile_size=TILE_SIZE, screen_size=pygdisp.get_window_size()
        )
                return MainGameInputHandler(world=world, rng=rng, bestiary=self.bestiary)


class LoadNewGameHandler(BaseInputHandler):
    def __init__(self, bestiary: Bestiary, rng: Random, parent: Handler | None = None):
        super().__init__(bestiary, parent)
        self.rng = rng
        self.q = Queue()
        self.process = Process(
            target=loaders.get_new_world,
            kwargs={
                "rng": self.rng,
                "q": self.q,
                "screen_size": pygdisp.get_window_size(),
            },
        )
        self.process.start()

    def handle_key(self, key, mod, unicode, scancode):
        if self.process.exitcode is None:
            return self
        elif self.process.exitcode == 0:
            world = dill.loads(lzma.decompress(self.q.get_nowait()))
            self.process.close()
            return MainGameInputHandler(
                world=world, rng=self.rng, bestiary=self.bestiary
            )
        else:
            raise AsyncException

    def render(
        self,
        *,
        surface: Surface,
        working_surface: Surface,
        sprites: dict[Sprites, Surface],
        fonts: dict[FontDict, freetype.Font],
    ):
        if self.process.exitcode is None:
            size = fonts[FontDict.MainMenu].get_rect(
                text=f"Loading{['.' for _ in range((time() * 4) % 4)]}"
            )
            dest = Rect((surface.get_width() - size.w) // 2, 35, 0, 0)
            fonts[FontDict.MainMenu].render_to(surface, dest, text=None)
        elif self.process.exitcode == 0:
            fonts[FontDict.MainMenu].render_to(
                surface, (0, 0), "Press any key to continue"
            )
        else:
            fonts[FontDict.MainMenu].render_to(
                surface, (0, 0), f"Error encountered! Exit code {self.process.exitcode}"
            )


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

    def render(
        self,
        *,
        surface: Surface,
        working_surface: Surface,
        sprites: dict[Sprites, Surface],
        fonts: dict[FontDict, freetype.Font],
    ):
        self.world.render(
            surface=surface,
            working_surface=working_surface,
            sprites=sprites,
            font=fonts[FontDict.GameStatus],
        )

    def handle_key(self, key, mod, unicode, scancode) -> Handler:
        match key:
            case move if move in MOVEMENT_KEYS:
                try:
                    BumpAction(
                        entity=self.world.player,
                        direction=MOVEMENT_KEYS[key],
                        gamemap=self.world.current_map,
                        rng=self.rng,
                    ).perform()
                    self.world.camera.set_center(
                        *self.world.player.components[Position].xy
                    )
                    self.world.current_map.update_player_fov()
                except PathBlocked:
                    self.world.message_log.add_message(
                        text="The way is blocked", color=colors.Impossible
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

    def render(
        self,
        *,
        surface: Surface,
        working_surface: Surface,
        sprites: dict[Sprites, Surface],
        fonts: dict[FontDict, freetype.Font],
    ):
        font = fonts[FontDict.MainMenu]
        gfxdraw.box(surface, (40, 60, 800, 600), (0xA0, 0x20, 0x70, 0xE0))
        font.render_to(
            surface,
            (10, 10),
            text="BESTIARY",
            fgcolor=(0xFF, 0xFF, 0xFF, 0xFF),
            bgcolor=(0x00, 0x00, 0x00, 0x00),
        )
