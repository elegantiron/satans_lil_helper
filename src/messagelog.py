"""Message log"""

from __future__ import annotations


class _Message:
    def __init__(self, text: str, color: tuple[int, int, int, int]) -> None:
        self.plain_text = text
        self.color = color
        self.count = 1

    @property
    def full_text(self) -> str:
        """Get this message's full text"""
        if self.count > 1:
            return f"{self.plain_text} (x{self.count})"
        return self.plain_text


class MessageLog:
    """Handle collecting and returning messages for the player"""

    def __init__(self) -> None:
        self.messages: list[_Message] = []

    def add_message(
        self,
        text: str,
        color: tuple[int, int, int, int],
        *,
        stack: bool = True,
    ) -> None:
        """Add a new message"""
        if stack and self.messages and text == self.messages[-1].plain_text:
            self.messages[-1].count += 1
        else:
            self.messages.append(_Message(text, color))

    def prune_message(self, text: str) -> None:
        if self.messages[-1].plain_text != text:
            raise RuntimeError
        if self.messages[-1].count > 1:
            self.messages[-1].count -= 1
        elif self.messages[-1].count == 1:
            self.messages.pop()
        else:
            raise RuntimeError
