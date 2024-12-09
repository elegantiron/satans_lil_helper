"""Tools for handling a `GameWorld`."""

from __future__ import annotations
from typing import TYPE_CHECKING

import numpy as np
import tcod.ecs

from components import Position, Renderable
from constants import CLASSES, MAPS, SPRITES, TAGS
from entity_factories import make_floor, make_wall, setup_player
from map_tools import CellularGenerator


if TYPE_CHECKING:
    from pygame import Surface


class GameWorld:
    def __init__(self):
        self._generators = {
            "cellular": CellularGenerator(
                width=MAPS.FOREST_WIDTH, height=MAPS.FOREST_HEIGHT
            ),
        }
        for key in self._generators:
            self.current_generator = key
            break
        self._worlds = []
        self.set_new_level()
        player = self.world.new_entity()
        setup_player(player, CLASSES.WARRIOR)
        self._player = player

    @property
    def world(self) -> tcod.ecs.Registry:
        return self._current_world

    @world.setter
    def world(self, value) -> None:
        raise AttributeError("The world attribute is read only.")

    @property
    def player(self):
        return self._player

    @player.setter
    def player(self, value) -> None:
        raise AttributeError("The player attribute is read only.")

    def new_level(self) -> None:
        """Generates a new level and adds it to the world.

        .. todo::
           - [ ] Add connections between the previous world and the new one
           - [ ] Place entities on the map"""
        new_world = tcod.ecs.Registry()
        self._worlds.append(new_world)
        map = self._generators[self.current_generator].get_new_map()
        for ix, iy in np.ndindex(map.shape):
            entity = new_world.new_entity()
            if map[ix, iy] == 0:
                make_floor(entity, ix, iy)
            else:
                make_wall(entity, ix, iy)

    def set_new_level(self, player: tcod.ecs.Entity = None) -> None:
        """Generates a new level and sets it as the current world.

        If `player` is supplied, that entity is moved from the old
        world to the new world."""
        self.new_level()
        if player is not None:
            self._player = player
            self._worlds[-1][player] = self._worlds[-2][player]
        self._current_world = self._worlds[-1]

    def render(self, surface: Surface, sprites: dict[SPRITES, Surface]) -> None:
        self._draw_map(surface, sprites)
        self._draw_status(surface, sprites)

    def _draw_map(self, surface: Surface, sprites: dict[SPRITES, Surface]) -> None:
        # blitlist = []
        # for entity, render, position in self.world.Q.all_of(
        #     components=[Renderable, Position], tags=[TAGS.TILE]
        # )[tcod.ecs.Entity, Renderable, Position]:
        #     blitlist.append(
        #         [
        #             sprites[entity.components[Renderable].image],
        #             (
        #                 entity.components[Position].x * 32,
        #                 entity.components[Position].y * 32,
        #             ),
        #         ]
        #     )
        blitlist = [
            [sprites[render.image], (position.x * 32, position.y * 32)]
            for render, position in self.world.Q.all_of(
                components=[Renderable, Position], tags=[TAGS.TILE]
            )[Renderable, Position]
            if (position.x < 40 and position.y < 23)
        ]

        for entity in self.world.Q.all_of(components=[Renderable, Position]).none_of(
            tags=[TAGS.TILE]
        ):
            blitlist.append(
                [
                    sprites[entity.components[Renderable].image],
                    (
                        entity.components[Position].x * 32,
                        entity.components[Position].y * 32,
                    ),
                ]
            )
        surface.blits(blitlist)

    def _draw_status(self, surface, sprites) -> None:
        pass
