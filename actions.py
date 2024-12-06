from __future__ import annotations

import esper

from components import IsItem, Position
from exceptions import MissingComponent


class Action:
    def __init__(self, entity):
        self.entity = entity

    def perform(self) -> None:
        raise NotImplementedError


class PickupAction(Action):
    def __init__(self, entity):
        super().__init__(entity)

    def perform(self) -> None:
        location = esper.try_component(self.entity, Position)
        if location is None:
            raise MissingComponent(
                "An entity without a position component is trying to pick up an item."
            )
        for ent in (
            entity
            for entity in esper.get_components(Position, IsItem)
            if (entity[1][0].x == location.x and entity[1][0].y == location.y)
        ):
            pass
