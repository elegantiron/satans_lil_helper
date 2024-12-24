from __future__ import annotations

from random import Random
from typing import TYPE_CHECKING, TypeVar

import pygame.freetype as freetype
import pygame.locals as Locals
from constants import MOVEMENT_KEYS, FontDict, Profession, Sprites, Strings

import game.colors as colors
import game.input_handlers.basehandler

from ..actions import BumpAction
from ..components import Position
from ..exceptions import PathBlocked
from ..gameworld import GameWorld

if TYPE_CHECKING:
    from pygame import Surface


Handler = TypeVar("Handler", bound="game.input_handlers.basehandler.BaseInputHandler")


class MainGameInputHandler(game.input_handlers.basehandler.BaseInputHandler):
    def __init__(
        self,
        world: GameWorld,
        rng: Random,
        player_class: Profession | None = Profession.Warrior,
        parent: Handler | None = None,
    ):
        super().__init__(parent=parent)
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
                        text=Strings.PathBlocked, color=colors.Impossible
                    )
                return self
            case Locals.K_ESCAPE:
                raise NotImplementedError
            case _:
                return self
