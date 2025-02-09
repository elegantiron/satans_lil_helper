"""Pickup Action"""

from __future__ import annotations

from components import Inventory, Position
from constants import EntityTags
from exceptions import InventoryFull, MissingComponent, NoItem

from .baseaction import BaseAction


class PickupAction(BaseAction):
    """Pick up items on the ground"""

    def perform(self) -> None:
        inventory = self.entity.components.get(Inventory, None)
        if inventory is None:
            raise MissingComponent

        location = self.entity.components.get(Position, None)
        if location is None:
            raise MissingComponent
        found_item = False
        for ent in (
            entity
            for entity in self.entity.registry.Q.all_of(
                components=[Position], tags=[EntityTags.ITEM]
            )
            if (entity.components[Position].xy == location.xy)
        ):
            if inventory.item_count >= inventory.size:
                raise InventoryFull
            ent.relation_tag[EntityTags.HELD_BY] = self.entity
            self.entity.relation_tags_many[EntityTags.HOLDING].add(ent)
            found_item = True
            inventory.item_count += 1

            del ent.components[Position]

        if not found_item:
            raise NoItem

    def rollback(self) -> None:
        raise NotImplementedError
