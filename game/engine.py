from __future__ import annotations

from random import Random
from time import time

import pygame
from pygame import Event

from game.constants import TILE_SIZE, GameSettings, HandlerActions, Strings
from game.gameworld import GameWorld
from game.input_handlers import (
    BestiaryInputHandler,
    GameMenuInputHandler,
    MainGameInputHandler,
    MainMenuInputHandler,
    SelectClassInputHandler,
)
from game.save_funcs import load_data, save_data


class Engine:
    world: GameWorld

    def __init__(self, bestiary):
        self.bestiary = bestiary
        self.handler = MainMenuInputHandler()

    def dispatch_event(self, event) -> None:
        result = None
        match event:
            case Event(type=pygame.QUIT):
                self._change_handler(MainGameInputHandler(self.world))
                save_data(self.bestiary, Strings.BestiaryPath)
                save_data(self.world, Strings.WorldPath)
                raise SystemExit
            case Event(type=pygame.KEYDOWN):
                result = self.handler.handle_key(
                    event.key, event.mod, event.unicode, event.scancode
                )
            case Event(type=pygame.MOUSEMOTION):
                result = self.handler.handle_mousemotion(
                    event.pos, event.rel, event.buttons, event.touch
                )
            case Event(type=pygame.MOUSEBUTTONDOWN):
                result = self.handler.handle_mousebuttondown(
                    event.pos, event.button, event.touch
                )
        match result:
            case HandlerActions.ShowBestiary:
                self._change_handler(BestiaryInputHandler(self.bestiary))
            case HandlerActions.ShowClassSelect:
                self._change_handler(SelectClassInputHandler())
            case HandlerActions.ShowGameMenu:
                self._change_handler(GameMenuInputHandler(self.handler))
            case HandlerActions.ShowGame:
                self._change_handler(MainGameInputHandler(self.world))
            case HandlerActions.ShowPrevious:
                self.handler = self._previous_handler
            case HandlerActions.ShowParent:
                self.handler = self.handler.parent
            case HandlerActions.LoadGame:
                self._load_game()
                self._change_handler(MainGameInputHandler(self.world))
            case HandlerActions.NewGame:
                self._new_game()
                self._change_handler(MainGameInputHandler(self.world))
            case HandlerActions.SaveAndQuit:
                self._change_handler(MainGameInputHandler(self.world))
                save_data(self.bestiary, Strings.BestiaryPath)
                save_data(self.world, Strings.WorldPath)
                raise SystemExit
            case _:
                pass

    def _change_handler(self, handler) -> None:
        self._previous_handler = self.handler
        self.handler = handler

    def _new_game(self) -> None:
        rng = Random(time())
        self.world = GameWorld(rng, TILE_SIZE, GameSettings.WindowSize)

    def render(self, surface, working_surface, sprites, fonts) -> None:
        self.handler.render(
            surface=surface,
            working_surface=working_surface,
            sprites=sprites,
            fonts=fonts,
        )

    def _load_game(self) -> None:
        self.world = load_data(Strings.WorldPath)
