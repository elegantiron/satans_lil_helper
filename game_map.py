from __future__ import annotations

from typing import Iterable, Iterator, List, Optional, TYPE_CHECKING

import numpy as np
from pygame import Surface
import tcod.camera

from entity import Actor, Item
import tile_types
from constants import CAMERA, SCREEN, STATUS


if TYPE_CHECKING:
    from engine import Engine
    from entity import Entity


class GameMap:
    def __init__(
        self, engine: Engine, width: int, height: int, entities: Iterable[Entity] = ()
    ):
        self.engine = engine
        self.width, self.height = width, height
        self.entities = set(entities)
        self.tiles = np.full((width, height), fill_value=tile_types.wall, order="F")

        self.visible = np.full((width, height), fill_value=False, order="F")
        self.explored = np.full((width, height), fill_value=False, order="F")
        self.downstairs_location = (0, 0)
        self.screen_array = np.arange(CAMERA.WIDTH * CAMERA.HEIGHT, dtype=int).reshape(
            CAMERA.WIDTH, CAMERA.HEIGHT
        )
        self.fog = Surface((32, 32))
        self.fog.set_alpha(0xB3)
        self.fog.fill((0, 0, 0))

        self.status_background = Surface((STATUS.WIDTH, STATUS.HEIGHT))
        self.status_background.set_alpha(0xA0)
        self.status_background.fill((0, 0, 0))

    @property
    def gamemap(self) -> GameMap:
        return self

    @property
    def actors(self) -> Iterator[Actor]:
        """Iterate over this map's living actors"""
        yield from (
            entity
            for entity in self.entities
            if isinstance(entity, Actor) and entity.is_alive
        )

    @property
    def items(self) -> Iterator[Item]:
        yield from (entity for entity in self.entities if isinstance(entity, Item))

    def get_blocking_entity_at_location(
        self,
        location_x: int,
        location_y: int,
    ) -> Optional[Entity]:
        for entity in self.entities:
            if (
                entity.blocks_movement
                and entity.x == location_x
                and entity.y == location_y
            ):
                return entity
        return None

    def get_actor_at_location(self, x: int, y: int) -> Optional[Actor]:
        for actor in self.actors:
            if actor.x == x and actor.y == y:
                return actor

        return None

    def in_bounds(self, x: int, y: int) -> bool:
        """Return True if x and y are inside of the bounds of this map"""
        return 0 <= x < self.width and 0 <= y < self.height

    def render(
        self,
        surface: Surface,
        tile_sprites: List[Surface],
        entity_sprites: List[Surface],
    ) -> None:
        """
        Renders the map.

        If a tile is in the "visible" array, then draw it normally.
        If it isn't, but it's in the "explored" array, then draw it shaded.
        Otherwise, leave the surface blank.
        """
        camera_i, camera_j = tcod.camera.get_camera(
            self.screen_array.shape, center=(self.engine.player.x, self.engine.player.y)
        )
        explored_tiles = np.select(
            condlist=[self.explored],
            choicelist=[self.tiles["sprite"].reshape(self.explored.shape)],
            default=-1,
        )
        tileblitlist = []
        fogblitlist = []
        entityblitlist = []
        for i in range(camera_i, min(self.width, camera_i + SCREEN.TILE_WIDTH)):
            for j in range(camera_j, min(self.width, camera_j + SCREEN.TILE_HEIGHT)):
                try:
                    if explored_tiles[i, j] != -1:
                        dest = ((i - camera_i) * 32, (j - camera_j) * 32)
                        tileblitlist.append((tile_sprites[explored_tiles[i, j]], dest))
                        # rect = surface.blit(tile_sprites[explored_tiles[i, j]], dest)
                        if not self.visible[i, j]:
                            fogblitlist.append((self.fog, dest))

                except IndexError:
                    pass

        entities_sorted_for_rendering = sorted(
            self.entities, key=lambda x: x.render_order.value
        )

        for entity in entities_sorted_for_rendering:
            if self.visible[entity.x, entity.y]:
                dest = ((entity.x - camera_i) * 32, (entity.y - camera_j) * 32)
                entityblitlist.append((entity_sprites[entity.img], dest))
                # surface.blit(entity_sprites[entity.img], dest)

        surface.blits(tileblitlist)
        surface.blits(fogblitlist)
        surface.blits(entityblitlist)
        surface.blit(self.status_background, (STATUS.WIDTH * 2, 0))

    def convert_points(self, pos: tuple[int, int]) -> tuple[int, int]:
        camera_i, camera_j = tcod.camera.get_camera(
            self.screen_array.shape, center=(self.engine.player.x, self.engine.player.y)
        )
        new_pos = (pos[0]//32 + camera_i, pos[1]//32 + camera_j)
        return new_pos


class GameWorld:
    """
    Holds the settings for the GameMap, and generates new maps when moving down the stairs.
    """

    def __init__(
        self,
        *,
        engine: Engine,
        map_width: int,
        map_height: int,
        max_rooms: int,
        room_min_size: int,
        room_max_size: int,
        current_floor: int = 0,
    ):
        self.engine = engine
        self.map_width = map_width
        self.map_height = map_height

        self.max_rooms = max_rooms
        self.room_min_size = room_min_size
        self.room_max_size = room_max_size

        self.current_floor = current_floor

    def generate_floor(self) -> None:
        from procgen import generate_rectangular

        self.current_floor += 1

        self.engine.game_map = generate_rectangular(
            max_rooms=self.max_rooms,
            room_min_size=self.room_min_size,
            room_max_size=self.room_max_size,
            map_width=self.map_width,
            map_height=self.map_height,
            engine=self.engine,
        )
