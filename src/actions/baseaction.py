"""Base action for all other actions to inherit"""

from __future__ import annotations

import abc
from typing import TYPE_CHECKING

from constants import Color

if TYPE_CHECKING:
    import tcod.ecs

    from messagelog import MessageLog


class BaseAction(metaclass=abc.ABCMeta):
    """Define base init and abstract functions"""

    message_log: MessageLog | None = None
    log_entry: str | None = None
    log_color: Color = Color.WHITE

    def __init__(self, entity: tcod.ecs.Entity, *, is_player: bool = False) -> None:
        self.entity = entity
        self.is_player = is_player

    @abc.abstractmethod
    def perform(self) -> None:
        """Perform the action in question."""
        if (
            self.is_player
            and self.message_log is not None
            and self.log_entry is not None
        ):
            self.message_log.add_message(self.log_entry, self.log_color)

    @abc.abstractmethod
    def rollback(self) -> None:
        """Undo the effects of this action."""
        if (
            self.is_player
            and self.message_log is not None
            and self.log_entry is not None
        ):
            self.message_log.prune_message(self.log_entry)
