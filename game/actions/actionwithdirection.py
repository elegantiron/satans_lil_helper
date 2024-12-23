from __future__ import annotations

from typing import TYPE_CHECKING

from ..components import Position
from ..gamemap import GameMap
from .action import Action

if TYPE_CHECKING:
    import tcod.ecs

class ActionWithDirection(Action):
    def __init__(
        self,
        entity: tcod.ecs.Entity,
        direction: tuple[int, int],
        gamemap: GameMap,
    ):
        super().__init__(entity=entity)
        self.gamemap = gamemap
        self.direction = direction
        position = entity.components[Position]
        self.target_xy = (self.dx + position.x, self.dy + position.y)
        self.target_entity = None
        for ent in (
            entity
            for entity in self.entity.registry.Q.all_of(components=[Position])
            if (
                entity.components[Position].x == self.target_x
                and entity.components[Position].y == self.target_y
            )
        ):
            self.target_entity = ent

    @property
    def dx(self) -> int:
        """change in the x axis"""
        return self.direction[0]

    @property
    def dy(self) -> int:
        """change in the y axis"""
        return self.direction[1]

    @property
    def target_x(self) -> int:
        """x coordinate of the target tile"""
        return self.target_xy[0]

    @property
    def target_y(self) -> int:
        """y coordinate of the target tile"""
        return self.target_xy[1]