"""Base action for all other actions to inherit"""

from __future__ import annotations
from typing import TYPE_CHECKING
import abc

if TYPE_CHECKING:
    import tcod.ecs


class Action(metaclass=abc.ABCMeta):
    """Define base init and abstract functions"""

    def __init__(self, entity: tcod.ecs.Entity):
        self.entity = entity

    @abc.abstractmethod
    def perform(self) -> None:
        """Perform the action in question.

        Subclasses **must** override this method."""
