"""Move Action"""

from __future__ import annotations

from typing import TYPE_CHECKING

import numpy as np

from components import Name, Position
from constants import Color, Tile
from exceptions import OutOfBounds, PathBlocked

from .actionwithdirection import ActionWithDirection

if TYPE_CHECKING:
    from tcod.ecs import Entity

    from gamemap import GameMap
    from messagelog import MessageLog


class MoveAction(ActionWithDirection):
    """Move an entity"""

    possible: bool = True

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
            self.possible = False
        elif not self.gamemap.tiles[Tile.WALKABLE][self.target_xy]:
            if not self.is_player:
                raise PathBlocked
            self.log_entry = "That way is blocked."
            self.log_color = Color.IMPOSSIBLE
            self.possible = False
        elif self.target_entity is not None:
            if not self.is_player:
                raise PathBlocked
            t_name = self.target_entity.components.get(
                Name, Name("mysterious stranger", indefinite_article="a")
            )
            name = (
                f"{t_name.indefinite_article.capitalize()} {t_name.name}"
                if t_name.indefinite_article != ""
                else f"{t_name.name.capitalize()}"
            )
            self.log_entry = f"{name} blocks your path."
            self.log_color = Color.IMPOSSIBLE
            self.possible = False

    def perform(self) -> None:
        if self.possible:
            position = self.entity.components[Position]
            position.x += self.dx
            position.y += self.dy
        super().perform()

    def rollback(self) -> None:
        if self.possible:
            position = self.entity.components[Position]
            position.x -= self.dx
            position.y -= self.dy
        super().rollback()
