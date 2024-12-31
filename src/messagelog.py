from __future__ import annotations

import textwrap
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from typing import Iterable


class _Message:
    def __init__(self, text: str, color):
        self.plain_text = text
        self.color = color
        self.count = 1

    @property
    def full_text(self) -> str:
        if self.count > 1:
            return f"{self.plain_text} (x{self.count})"
        return self.plain_text


class MessageLog:
    def __init__(self) -> None:
        self.messages: list[_Message] = []

    def add_message(
        self,
        text: str,
        color,
        *,
        stack: bool = True,
    ) -> None:
        if stack and self.messages and text == self.messages[-1].plain_text:
            self.messages[-1].count += 1
        else:
            self.messages.append(_Message(text, color))

    @staticmethod
    def wrap(string: str, width: int) -> Iterable[str]:
        for line in string.splitlines():
            yield from textwrap.wrap(line, width, expand_tabs=True)
