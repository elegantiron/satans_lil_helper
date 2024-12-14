from __future__ import annotations
from typing import TYPE_CHECKING

import tcod.ecs
from pygame import Surface
from pyrotkit.generators import CellularGenerator, MapGenerator
from pyrotkit.tools import Camera

from constants import MAPS

if TYPE_CHECKING:
    import numpy.typing as npt

class GameWorld:
    _current_world: tcod.ecs.Registry
    _player: tcod.ecs.Entity
    _camera: Camera
    _sprites: dict[str, Surface]
    _map: npt.NDArray

    def __init__(self, generators: dict[str, MapGenerator] = None, surface: Surface = None):
        if generators is not None:
            self._generators = generators
        else:
            self._generators = {
                "forest": CellularGenerator(
                    width=MAPS.FOREST_WIDTH, height=MAPS.FOREST_HEIGHT
                )
            }
        self.surface = surface
        self._worlds = []

    @property
    def world(self) -> tcod.ecs.Registry:
        return self._current_world

    @world.setter
    def world(self, _) -> None:
        raise AttributeError("The world attribute is read only.")

    @property
    def player(self) -> tcod.ecs.Entity:
        return self._player

    @player.setter
    def player(self, _) -> None:
        raise AttributeError("The player attribute is read only.")
    
    def render(self) -> None:
        pass

    def _draw_map(self) -> None:
        pass