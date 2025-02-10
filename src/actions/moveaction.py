"""Move Action"""

from __future__ import annotations

from typing import TYPE_CHECKING

import numpy as np

from components import Position
from constants import Color, Tile
from exceptions import OutOfBounds, PathBlocked

from .actionwithdirection import ActionWithDirection

if TYPE_CHECKING:
    from tcod.ecs import Entity

    from gamemap import GameMap
    from messagelog import MessageLog


class MoveAction(ActionWithDirection):
    """Move an entity"""

    def __init__(
        self,
        entity: Entity,
        direction: tuple[int, int],
        gamemap: GameMap,
        message_log: MessageLog,
        *,
        is_player: bool = False,
    ) -> None:
        super().__init__(entity, direction, gamemap, message_log, is_player=is_player)
        if (self.target_x, self.target_y) not in np.ndindex(self.gamemap.tiles.shape):
            if not self.is_player:
                raise OutOfBounds
            self.log_entry = "There is nothing but the Void in that direction."
            self.log_color = Color.IMPOSSIBLE
        if not self.gamemap.tiles[Tile.WALKABLE][self.target_xy]:
            raise PathBlocked
        if self.target_entity is not None:
            raise PathBlocked

    def perform(self) -> None:
        position = self.entity.components[Position]
        position.x += self.dx
        position.y += self.dy
        if (
            self.is_player
            and self.message_log is not None
            and self.log_entry is not None
        ):
            self.message_log.add_message(self.log_entry, self.log_color)

    def rollback(self) -> None:
        position = self.entity.components[Position]
        position.x -= self.dx
        position.y -= self.dy
        if (
            self.is_player
            and self.message_log is not None
            and self.log_entry is not None
        ):
            self.message_log.prune_message(self.log_entry)
