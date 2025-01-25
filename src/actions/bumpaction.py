"""Bump Action"""

from __future__ import annotations

from typing import TYPE_CHECKING

from constants import EntityTags

from .actionwithdirection import ActionWithDirection
from .meleeaction import MeleeAction
from .moveaction import MoveAction

if TYPE_CHECKING:
    import random

    import tcod.ecs

    from gamemap import GameMap


class BumpAction(ActionWithDirection):
    """'Bumps' a tile and performs the appropriate action"""

    def __init__(
        self,
        entity: tcod.ecs.Entity,
        direction: tuple[int, int],
        gamemap: GameMap,
        rng: random.Random,
    ) -> None:
        super().__init__(entity, direction, gamemap)
        self.rng = rng

    def perform(self) -> None:
        if self.target_entity:
            if EntityTags.HOSTILE in self.target_entity.tags:
                MeleeAction(
                    entity=self.entity,
                    direction=self.direction,
                    gamemap=self.gamemap,
                    rng=self.rng,
                ).perform()
        else:
            MoveAction(
                entity=self.entity, direction=self.direction, gamemap=self.gamemap
            ).perform()
