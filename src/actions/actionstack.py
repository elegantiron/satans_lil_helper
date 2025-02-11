from __future__ import annotations

import copy
from typing import TYPE_CHECKING

from constants import Tile

from .actionwithdirection import ActionWithDirection

if TYPE_CHECKING:
    import numpy.typing as npt

    from messagelog import MessageLog

    from .baseaction import BaseAction


class _ActionStackFrame:
    tile_state: npt.NDArray | None = None

    def __init__(
        self,
        action: ActionWithDirection | BaseAction,
    ) -> None:
        self.action = action
        if isinstance(action, ActionWithDirection):
            self.tile_state = copy.copy(action.gamemap.tiles)

    def perform(self) -> None:
        self.action.perform()

    def rollback(self) -> None:
        self.action.rollback()
        if isinstance(self.action, ActionWithDirection) and self.tile_state is not None:
            self.action.gamemap.tiles[Tile.EXPLORED] = copy.copy(self.tile_state[Tile.EXPLORED])


class ActionStack:
    def __init__(self):
        self._stack: list[_ActionStackFrame] = []
        self._idx: int = -1

    @property
    def idx(self) -> int:
        return self._idx

    @property
    def len(self) -> int:
        return len(self._stack)

    @property
    def current_frame(self) -> _ActionStackFrame:
        return self._stack[self.idx]

    def add_action(
        self,
        action: BaseAction,
    ) -> None:
        """Adds an action to the stack and performs it"""
        new_frame = _ActionStackFrame(action)
        if self.idx != self.len - 1:
            del self._stack[self.idx + 1 :]
        self._stack.append(new_frame)
        self._idx = self.len - 1
        self.current_frame.perform()

    def undo_action(self, message_log: MessageLog) -> None:
        if self.idx >= 0:
            self.current_frame.rollback()
            self._idx -= 1

    def redo_action(self):
        if self.len - 1 > self.idx:
            self._idx += 1
            self.current_frame.perform()
