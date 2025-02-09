"""Base action for all other actions to inherit"""

from __future__ import annotations

import abc
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    import tcod.ecs

    from messagelog import MessageLog


class BaseAction(metaclass=abc.ABCMeta):
    """Define base init and abstract functions"""

    message_log: MessageLog | None = None

    def __init__(self, entity: tcod.ecs.Entity) -> None:
        self.entity = entity

    @abc.abstractmethod
    def perform(self) -> None:
        """Perform the action in question."""

    @abc.abstractmethod
    def rollback(self) -> None:
        """Undo the effects of this action."""

    def get_text(self) -> str | None:
        return None
