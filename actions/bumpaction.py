from __future__ import annotations

from typing import TYPE_CHECKING

from constants import Tags

from .actionwithdirection import ActionWithDirection
from .meleeaction import MeleeAction
from .moveaction import MoveAction

if TYPE_CHECKING:
    import random


class BumpAction(ActionWithDirection):
    def __init__(self, entity, direction, gamemap, rng: random.Random):
        super().__init__(entity, direction, gamemap)
        self.rng = rng

    def perform(self) -> None:
        if self.target_entity:
            if Tags.Hostile in self.target_entity.tags:
                return MeleeAction(
                    entity=self.entity,
                    direction=self.direction,
                    gamemap=self.gamemap,
                    rng=self.rng,
                ).perform()
            elif Tags.Friendly in self.target_entity.tags:
                pass
        else:
            return MoveAction(
                entity=self.entity, direction=self.direction, gamemap=self.gamemap
            ).perform()
