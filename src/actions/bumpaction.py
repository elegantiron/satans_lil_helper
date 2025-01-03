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
    def __init__(
        self,
        entity: tcod.ecs.Entity,
        direction: tuple[int, int],
        gamemap: GameMap,
        rng: random.Random,
    ):
        super().__init__(entity, direction, gamemap)
        self.rng = rng

    def perform(self) -> None:
        if self.target_entity:
            if EntityTags.Hostile in self.target_entity.tags:
                return MeleeAction(
                    entity=self.entity,
                    direction=self.direction,
                    gamemap=self.gamemap,
                    rng=self.rng,
                ).perform()
            elif EntityTags.Friendly in self.target_entity.tags:
                pass
        else:
            return MoveAction(
                entity=self.entity, direction=self.direction, gamemap=self.gamemap
            ).perform()
