from __future__ import annotations

from typing import TYPE_CHECKING

from constants import Color

if TYPE_CHECKING:
    from messagelog import MessageLog

    from .actionwithdirection import ActionWithDirection
    from .baseaction import BaseAction


class _ActionStackFrame:
    def __init__(
        self,
        action: ActionWithDirection | BaseAction,
        text: str | None = None,
        color: Color = Color.WHITE,
    ) -> None:
        self.action = action
        self.text = text
        self.color = color

    def perform(self) -> None:
        self.action.perform()
        if self.action.message_log is not None and self.text is not None:
            self.action.message_log.add_message(self.text, self.color)

    def rollback(self) -> None:
        self.action.rollback()
        if self.action.message_log is not None and self.text is not None:
            self.action.message_log.prune_message(self.text)


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
        text: str | None = None,
        color: Color = Color.WHITE,
    ) -> None:
        """Adds an action to the stack and performs it"""
        new_frame = _ActionStackFrame(action, text, color)
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
