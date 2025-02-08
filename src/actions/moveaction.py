"""Move Action"""

from __future__ import annotations

import numpy as np

from components import Position
from constants import Tile
from exceptions import OutOfBounds, PathBlocked

from .actionwithdirection import ActionWithDirection


class MoveAction(ActionWithDirection):
    """Move an entity"""

    def perform(self) -> None:
        if (self.target_x, self.target_y) not in np.ndindex(self.gamemap.tiles.shape):
            raise OutOfBounds
        if not self.gamemap.tiles[Tile.WALKABLE][self.target_xy]:
            raise PathBlocked
        if self.target_entity is not None:
            raise PathBlocked

        position = self.entity.components[Position]
        position.x += self.dx
        position.y += self.dy

    def rollback(self) -> None:
        position = self.entity.components[Position]
        position.x -= self.dx
        position.y -= self.dy
