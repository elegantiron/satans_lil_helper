from __future__ import annotations

import tcod.ecs

from constants import CLASSES
from utils import setup_player


class GameWorld:
    def __init__(self):
        self._worlds = []
        self.set_new_level()
        player = self.world.new_entity()
        setup_player(player, CLASSES.WARRIOR)
        

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
        """Generates a new level and adds it to the world."""
        self._worlds.append(tcod.ecs.Registry())
        # TODO Generate a new map here

    def set_new_level(self, player: tcod.ecs.Entity = None) -> None:
        """Generates a new level and sets it as the current world.

        If `player` is supplied, that entity is moved from the old
        world to the new world."""
        self.new_level()
        if player is not None:
            self._player = player
            self._worlds[-1][player] = self._worlds[-2][player]
        self._current_world = self._worlds[-1]

    def render(self, surface, sprites) -> None:
        self._draw_map(surface, sprites)
        self._draw_status(surface, sprites)

    def _draw_map(self, surface, sprites) -> None:
        pass

    def _draw_status(self, surface, sprites) -> None:
        pass
