from __future__ import annotations

import numpy as np

from ..components import Position
from ..constants import Tile
from ..exceptions import PathBlocked
from .actionwithdirection import ActionWithDirection


class MoveAction(ActionWithDirection):
    def perform(self) -> None:
        if (self.target_x, self.target_y) not in np.ndindex(self.gamemap.tiles.shape):
            raise PathBlocked
        if not self.gamemap.tiles[Tile.Walkable][self.target_xy]:
            raise PathBlocked
        if self.target_entity is not None:
            raise PathBlocked

        position = self.entity.components[Position]
        position.x += self.dx
        position.y += self.dy
