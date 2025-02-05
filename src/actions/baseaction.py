"""Base action for all other actions to inherit"""

from __future__ import annotations

import abc
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    import tcod.ecs


class Action(metaclass=abc.ABCMeta):
    """Define base init and abstract functions"""

    def __init__(self, entity: tcod.ecs.Entity) -> None:
        self.entity = entity

    @abc.abstractmethod
    def perform(self) -> None:
        """Perform the action in question."""

    @abc.abstractmethod
    def rollback(self) -> None:
        """Undo the effects of this action."""
