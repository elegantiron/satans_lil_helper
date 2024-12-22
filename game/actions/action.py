from __future__ import annotations

from typing import TYPE_CHECKING

if TYPE_CHECKING:
    import tcod.ecs

class Action:
    def __init__(self, entity: tcod.ecs.Entity):
        self.entity = entity

    def perform(self) -> None:
        raise NotImplementedError