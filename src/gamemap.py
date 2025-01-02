from __future__ import annotations

from typing import TYPE_CHECKING, Generator

import numpy as np
import tcod
import tcod.constants
import tcod.ecs

from components import ActionDelay, Position, Stats
from constants import Tile
from utils import move_entity

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
        self.player.components[Position] = Position(
            x=5, y=5, sprite=":images:player/player.png"
        )
        self.player.components[Stats] = Stats(hp=30, mp=5, strength=5, magic=5, pdef=5, mdef=5, evasion=5, crit=5, sight=7, light=7)
        self.player.components[ActionDelay] = ActionDelay(0)

    def add_player(self, player: tcod.ecs.Entity) -> None:
        move_entity(player, self.registry)

    def initialize_pathfinder(self) -> None:
        self.graph = tcod.path.SimpleGraph(
            cost=self.tiles[Tile.MovementCost], cardinal=2, diagonal=3
        )
        self.pathfinder = tcod.path.Pathfinder(self.graph)

    def get_fov(
        self,
        pov: tuple[int, int],
        radius: int,
        light_walls: bool = True,
        algorithm: int = tcod.constants.FOV_SHADOW,
    ) -> npt.NDArray[np.bool_]:
        # print(self.tiles[Tile.Transparent])
        return tcod.map.compute_fov(
            transparency=self.tiles[Tile.Transparent],
            pov=pov,
            radius=radius,
            light_walls=light_walls,
            algorithm=algorithm,
        )
