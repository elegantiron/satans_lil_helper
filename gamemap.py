from __future__ import annotations

from typing import Any, Generator

import numpy as np
import numpy.typing as npt
import tcod.ecs
from components import Position

from utils import move_entity


class GameMap:
    registry: tcod.ecs.Registry
    player: tcod.ecs.Entity
    tiles: npt.NDArray
    generator: Any

    @property
    def tile_list(self) -> Generator[npt.DTypeLike]:
        for ix, iy in np.ndindex(self.tiles.shape):
            yield self.tiles[ix, iy]

    def __init__(self):
        self.registry = tcod.ecs.Registry()

    def new_player(self) -> None:
        self.player = self.registry.new_entity()
        self.player.components[Position] = Position(
            x=5, y=5, sprite=":images:player/player.png"
        )

    def add_player(self, player: tcod.ecs.Entity) -> None:
        move_entity(player, self.registry)
