"""The entire world"""

from __future__ import annotations

import time
from random import Random
from typing import TYPE_CHECKING

import numpy as np

from ai_helpers import confused_action, hostile_action, wander_action
from components import (
    AI,
    ActionDelay,
    Confusion,
    DamagingAilment,
    Position,
    Regen,
    Stats,
)
from constants import AIType, EntityTags, Tile
from exceptions import Impossible, MissingComponent, SpawnBlocked
from gamemap import GameMap
from tile_types import ForestFloor, ForestWall
from utils import get_neighbors

if TYPE_CHECKING:
    from collections.abc import Callable

    import tcod.ecs

    from messagelog import MessageLog


class GameWorld:
    """The entire world, and the methods to make new maps"""

    BIRTH_LIMIT = 4
    DEATH_LIMIT = 3
    WALL_CHANCE = 0.44
    MAP_X = 100
    MAP_Y = 100

    def __init__(self, seed: int | float | None = None) -> None:
        self._current_map = GameMap()
        self._maps = [self._current_map]
        self.map_index = self._maps.index(self._current_map)
        self.rng = Random(seed if seed is not None else time.time())
        tiles = [
            [
                (1 if self.rng.random() < self.WALL_CHANCE else 0)
                for _ in range(self.MAP_X)
            ]
            for _ in range(self.MAP_Y)
        ]
        self.map.sprites = [
            [None for _ in range(self.MAP_X)] for _ in range(self.MAP_Y)
        ]
        for _ in range(4):
            new_tiles = [[0 for _ in range(self.MAP_X)] for _ in range(self.MAP_Y)]
            # pylint: disable=consider-using-enumerate
            for ix in range(len(tiles)):
                for iy in range(len(tiles[ix])):
                    neighbors = get_neighbors(ix, iy, tiles)
                    if neighbors > self.BIRTH_LIMIT:
                        new_tiles[ix][iy] = 1
            # pylint: enable=consider-using-enumerate
            tiles = new_tiles

        self._current_map.tiles = np.full(
            (self.MAP_X, self.MAP_Y), fill_value=ForestFloor, order="F"
        )
        for ix, iy in np.ndindex(self._current_map.tiles.shape):
            if tiles[ix][iy] == 0:
                self._current_map.tiles[ix, iy] = ForestFloor
            else:
                self._current_map.tiles[ix, iy] = ForestWall
        self._current_map.new_player()
        p_pos = self.player.components[Position]
        while not self.map.tiles[Tile.WALKABLE][p_pos.xy]:
            if self.rng.random() < 0.5:
                p_pos.x += 1
            else:
                p_pos.y += 1

    def __eq__(self, other: object) -> bool:
        if not isinstance(other, GameWorld):
            raise TypeError
        return self._maps == other._maps and self.rng.getstate() == other.rng.getstate()

    @property
    def map(self) -> GameMap:
        """The current map"""
        return self._current_map

    @property
    def registry(self) -> tcod.ecs.Registry:
        """The active registry"""
        return self.map.registry

    @property
    def player(self) -> tcod.ecs.Entity:
        """The player entity"""
        return self.map.player

    def step_time(self) -> None:
        for ent in self.registry.Q.all_of(components=[ActionDelay]):
            ent.components[ActionDelay].ticks -= 1

    def process_ai(self, message_log: MessageLog) -> None:
        for ent in self.registry.Q.all_of(components=[Position, AI, ActionDelay]):
            if ent.components[ActionDelay].ticks <= 0:
                action_complete = False
                while not action_complete:
                    try:
                        match ent.components[AI].type:
                            case AIType.WANDERING:
                                wander_action(ent, self.map, self.rng)
                                action_complete = True
                            case AIType.CONFUSED:
                                confusion = ent.components.get(Confusion, None)
                                if confusion is None:
                                    raise MissingComponent
                                if confusion.age < confusion.limit:
                                    confused_action(ent, self.map, self.rng, message_log)
                                    confusion.age += 1
                                    action_complete = True
                                else:
                                    ent.components[AI].type = ent.components[
                                        AI
                                    ].base_type
                            case AIType.HOSTILE:
                                hostile_action(ent, self.map, self.rng, message_log)
                                action_complete = True
                            case AIType.HOWL_RESPONSE:
                                pass
                    except Impossible:
                        # Catch impossible actions and ignore them.
                        # We don't care if the AI tries something it can't do
                        pass

    def handle_regen(self):
        for ent in self.registry.Q.all_of(components=[Stats, Regen]):
            stats = ent.components[Stats]
            regen = ent.components[Regen]
            regen.counter += 1
            if regen.proc:
                stats.hp += regen.health
                stats.mp += regen.mana

    def handle_ailments(self):
        for ent in self.registry.Q.all_of(components=[DamagingAilment]):
            target = ent.relation_tag[EntityTags]
            dice, sides = ent.components[DamagingAilment].damage
            damage = 0
            for _ in range(dice):
                damage += self.rng.randint(1, sides)
            stats = target.components.get(Stats, None)
            if stats is None:
                continue
            stats.hp -= damage

    def spawn_entity(
        self,
        setup_function: Callable[[tcod.ecs.Entity, tuple[int, int], Random], None],
        position: tuple[int, int] | None = None,
    ) -> tcod.ecs.Entity:
        entity = self.map.registry.new_entity()
        if position is None:
            x: int = 0
            y: int = 0
            chosen = False
            while not chosen:
                x = self.rng.randint(0, len(self.map.tiles) - 1)
                y = self.rng.randint(0, len(self.map.tiles[x]) - 1)
                if self.map.tiles[Tile.WALKABLE][x, y]:
                    chosen = True
            position = (x, y)
        if not self.map.tiles[Tile.WALKABLE][position]:
            raise SpawnBlocked
        setup_function(entity, position, self.rng)
        return entity
