from __future__ import annotations

from ..components import Inventory, Position
from ..constants import Tags
from ..exceptions import InventoryFull, MissingComponent
from .baseaction import Action

class PickupAction(Action):
    def perform(self) -> None:
        inventory = self.entity.components.get(Inventory, None)
        if inventory is None:
            raise MissingComponent
        
        location = self.entity.components.get(Position, None)
        if location is None:
            raise MissingComponent
        for ent in (
            entity
            for entity in self.entity.registry.Q.all_of(
                components=[Position]
            )
            if (entity.components[Position].xy == location.xy)
        ):
            if (
                len(
                    set(
                        self.entity.registry.Q.all_of(
                            relations=[self.entity, Tags.Holding, None]
                        )
                    )
                )
                >= inventory.size
            ):
                raise InventoryFull
            ent.relation_tag[Tags.HeldBy] = self.entity
            self.entity.relation_tags_many[Tags.Holding].add(ent)

            del ent[Position]