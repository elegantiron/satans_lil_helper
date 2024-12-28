from __future__ import annotations
from typing import TYPE_CHECKING
import abc

if TYPE_CHECKING:
    import tcod.ecs


class Action(metaclass=abc.ABCMeta):
    def __init__(self, entity: tcod.ecs.Entity):
        self.entity = entity

    @abc.abstractmethod
    def perform(self) -> None:
        pass
