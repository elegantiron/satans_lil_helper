"""A single level of the world"""
from __future__ import annotations

from typing import TYPE_CHECKING

import numpy as np
import tcod
import tcod.constants
import tcod.ecs

from constants import Tile
from entities import professions
from utils import move_entity

if TYPE_CHECKING:
    from collections.abc import Generator

    import arcade
    import numpy.typing as npt


class GameMap:
    """Represents a level of the world"""
    registry: tcod.ecs.Registry
    player: tcod.ecs.Entity
    tiles: npt.NDArray
    pathfinder: tcod.path.Pathfinder
    sprites: list[list[arcade.Sprite]]

    @property
    def tile_list(self) -> Generator[npt.DTypeLike]:
        """The map's tiles as a list"""
        for ix, iy in np.ndindex(self.tiles.shape):
            yield self.tiles[ix, iy]

    def __init__(self) -> None:
        self.registry = tcod.ecs.Registry()

    def new_player(self) -> None:
        """Make a new player"""
        self.player = self.registry.new_entity()
        professions.warrior_class(self.player)

    def bring_player(self, player: tcod.ecs.Entity) -> None:
        """Bring the player from another map"""
        move_entity(player, self.registry)

    def get_fov(
        self,
        pov: tuple[int, int],
        radius: int,
        *,
        light_walls: bool = True,
        algorithm: int = tcod.constants.FOV_SHADOW,
    ) -> npt.NDArray[np.bool_]:
        """Get the tiles currently visible from an arbitrary tile"""
        return tcod.map.compute_fov(
            transparency=self.tiles[Tile.TRANSPARENT],
            pov=pov,
            radius=radius,
            light_walls=light_walls,
            algorithm=algorithm,
        )
