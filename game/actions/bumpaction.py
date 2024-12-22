from __future__ import annotations
from .actionwithdirection import ActionWithDirection
from .meleeaction import MeleeAction
from .movementaction import MovementAction
from ..constants import Tags
from ..gamemap import GameMap
from typing import TYPE_CHECKING
if TYPE_CHECKING:
    from random import Random
    import tcod.ecs

class BumpAction(ActionWithDirection):
    def __init__(
        self,
        entity: tcod.ecs.Entity,
        direction: tuple[int, int],
        gamemap: GameMap,
        rng: Random,
    ):
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
                # return TalkAction(
                    # entity=self.entity, direction=self.direction, gamemap=self.gamemap
                # ).perform()
                pass
        else:
            return MovementAction(
                entity=self.entity, direction=self.direction, gamemap=self.gamemap
            ).perform()