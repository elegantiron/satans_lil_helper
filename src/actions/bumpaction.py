"""Bump Action"""

from __future__ import annotations

from typing import TYPE_CHECKING

from .actionwithdirection import ActionWithDirection
from .meleeaction import MeleeAction
from .moveaction import MoveAction

if TYPE_CHECKING:
    import random

    import tcod.ecs

    from gamemap import GameMap
    from messagelog import MessageLog


class BumpAction(ActionWithDirection):
    """'Bumps' a tile and performs the appropriate action"""

    def __init__(
        self,
        entity: tcod.ecs.Entity,
        direction: tuple[int, int],
        gamemap: GameMap,
        rng: random.Random,
        message_log: MessageLog,
    ) -> None:
        super().__init__(entity, direction, gamemap, message_log)
        self.rng = rng
        self.message_log = message_log

    def perform(self) -> None:
        if self.target_entity:
            MeleeAction(
                entity=self.entity,
                direction=self.direction,
                gamemap=self.gamemap,
                rng=self.rng,
                message_log=self.message_log,
            ).perform()
        else:
            MoveAction(
                entity=self.entity,
                direction=self.direction,
                gamemap=self.gamemap,
                message_log=self.message_log,
            ).perform()
