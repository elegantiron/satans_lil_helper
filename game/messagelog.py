from __future__ import annotations
import textwrap
from typing import TYPE_CHECKING


if TYPE_CHECKING:
    from pygame import Color, Rect, Surface
    from pygame.freetype import Font
    from typing import Iterable, Reversible


class Message:
    def __init__(self, text: str, color: Color):
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
        self.messages: list[Message] = []

    def add_message(
        self,
        text: str,
        color: Color,
        *,
        stack: bool = True,
    ) -> None:
        if stack and self.messages and text == self.messages[-1].plain_text:
            self.messages[-1].count += 1
        else:
            self.messages.append(Message(text, color))

    def render(
            self, surface: Surface, rect: Rect, font: Font
    ):
        self.render_messages(surface, rect, font, self.messages)

    @staticmethod
    def wrap(string: str, width: int) -> Iterable[str]:
        for line in string.splitlines():
            yield from textwrap.wrap(
                line, width, expand_tabs=True
            )

    @classmethod
    def render_messages(
        cls,
        surface: Surface,
        rect: Rect,
        messages: Reversible[Message],
    ) -> None:
        y_offset = rect.h-1
        y_offset += y_offset
        for message in reversed(messages):
            for line in reversed(list(cls.wrap(message.full_text, rect.w))):
                # TODO: render the messages
                pass