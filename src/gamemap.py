from __future__ import annotations

from typing import TYPE_CHECKING, Generator

import numpy as np
import tcod
import tcod.constants
import tcod.ecs

from constants import Tile
from utils import move_entity
from entities import professions

if TYPE_CHECKING:
    import arcade
    import numpy.typing as npt


class GameMap:
    registry: tcod.ecs.Registry
    player: tcod.ecs.Entity
    tiles: npt.NDArray
    pathfinder: tcod.path.Pathfinder
    sprites: list[list[arcade.Sprite]]

    @property
    def tile_list(self) -> Generator[npt.DTypeLike]:
        for ix, iy in np.ndindex(self.tiles.shape):
            yield self.tiles[ix, iy]

    def __init__(self):
        self.registry = tcod.ecs.Registry()
        self.sprites = None

    def new_player(self) -> None:
        self.player = self.registry.new_entity()
        professions.warrior_class(self.player)

    def bring_player(self, player: tcod.ecs.Entity) -> None:
        move_entity(player, self.registry)

    def get_fov(
        self,
        pov: tuple[int, int],
        radius: int,
        light_walls: bool = True,
        algorithm: int = tcod.constants.FOV_SHADOW,
    ) -> npt.NDArray[np.bool_]:
        return tcod.map.compute_fov(
            transparency=self.tiles[Tile.Transparent],
            pov=pov,
            radius=radius,
            light_walls=light_walls,
            algorithm=algorithm,
        )
