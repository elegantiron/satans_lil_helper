from __future__ import annotations

from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .baseaction import Action


class ActionQueue:
    def __init__(self):
        self._stack: list[Action] = []
        self._idx: int = -1

    @property
    def idx(self) -> int:
        return self._idx

    @idx.setter
    def idx(self, value: int) -> None:
        if not isinstance(value, int):
            raise TypeError
        self._idx = value

    def push_action(self, action):
        self._stack.append(action)
        self._idx = len(self._stack) - 1

    def undo_action(self):
        if self.idx >= 0:
            self._stack[self.idx].rollback()
            self.idx -= 1
