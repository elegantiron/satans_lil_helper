"""Base directional action"""

from __future__ import annotations

from abc import ABCMeta
from typing import TYPE_CHECKING

from components import Position

from .baseaction import BaseAction

if TYPE_CHECKING:
    import tcod.ecs

    from gamemap import GameMap
    from messagelog import MessageLog


class ActionWithDirection(BaseAction, metaclass=ABCMeta):
    """Base class for directional actions"""
    log_entry: str | None = None
    def __init__(
        self,
        entity: tcod.ecs.Entity,
        direction: tuple[int, int],
        gamemap: GameMap,
        message_log: MessageLog,
        *,
        is_player: bool = False
    ) -> None:
        super().__init__(entity=entity, is_player=is_player)
        self.gamemap = gamemap
        self.direction = direction
        position = entity.components[Position]
        self.target_xy = self.dx + position.x, self.dy + position.y
        self.target_entity = None
        self.message_log = message_log
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
        """Difference in x axis"""
        return self.direction[0]

    @property
    def dy(self) -> int:
        """Difference in y axis"""
        return self.direction[1]

    @property
    def target_x(self) -> int:
        """Target's x coordinate"""
        return self.target_xy[0]

    @property
    def target_y(self) -> int:
        """Target's y coordinate"""
        return self.target_xy[1]
