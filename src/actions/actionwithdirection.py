from __future__ import annotations

from typing import TYPE_CHECKING
from .baseaction import Action
from components import Position

if TYPE_CHECKING:
    import tcod.ecs
    from gamemap import GameMap

class ActionWithDirection(Action):
    def __init__(
            self, entity: tcod.ecs.Entity,
            direction: tuple[int, int],
            gamemap: GameMap,
    ):
        super().__init__(entity=entity)
        self.gamemap = gamemap
        self.direction = direction
        position = entity.components[Position]
        self.target_xy = self.dx + position.x, self.dy + position.y
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
        return self.direction[0]
    
    @property
    def dy(self) -> int:
        return self.direction[1]
    
    @property
    def target_x(self) -> int:
        return self.target_xy[0]
    
    @property
    def target_y(self) -> int:
        return self.target_xy[1]