from typing import Iterable, List, Reversible, Tuple
import textwrap

import color
from pygame import Surface
import pygame.freetype

from constants import STATUS
import render_functions


class Message:
    def __init__(self, text: str, fg: Tuple[int, int, int]):
        self.plain_text = text
        self.fg = fg
        self.count = 1

    @property
    def full_text(self) -> str:
        """The full text of this message, including the count if necessary."""
        if self.count > 1:
            return f"{self.plain_text} (x{self.count})"
        return self.plain_text


class MessageLog:
    def __init__(self) -> None:
        self.messages: List[Message] = []

    def add_message(
        self,
        text: str,
        fg: Tuple[int, int, int] = color.white,
        *,
        stack: bool = True,
    ) -> None:
        """
        Add a message to this log.

        `text` is the message text, `fg` is the text color.

        If `stack` is True, then the message can stack with a
        previous message of the same text.
        """
        if stack and self.messages and text == self.messages[-1].plain_text:
            self.messages[-1].count += 1
        else:
            self.messages.append(Message(text, fg))

    def render(
        self,
        surface: Surface,
        font: pygame.freetype.Font,
        x: int,
        y: int,
        width: int,
        height: int,
    ) -> None:
        """
        Render this log over the given area.

        `x`, `y`, `width`, `height` is the rectangular region to render
        onto the surface.
        """
        self.render_messages(surface, font, x, y, width, height, self.messages)

    @staticmethod
    def wrap(string: str, width: int) -> Iterable[str]:
        """Return a wrapped text message."""
        for line in string.splitlines():
            yield from textwrap.wrap(line, width, expand_tabs=True)

    @classmethod
    def render_messages(
        cls,
        surface: Surface,
        font: pygame.freetype.Font,
        x: int,
        y: int,
        width: int,
        height: int,
        messages: Reversible[Message],
    ) -> None:
        """
        Render the messages provided.

        The `messages` are rendered starting at the latest message
        and working backwards.
        """
        y_offset = height - 1
        position = (int(STATUS.WIDTH * 2.1), int(STATUS.HEIGHT * 0.65))
        for message in reversed(messages):
            for line in list(cls.wrap(message.full_text, width)):
                position = render_functions.render_line(
                    text=line,
                    font=font,
                    surface=surface,
                    position=position,
                    fgcolor=message.fg,
                    bgcolor=(0, 0, 0),
                )
                y_offset -= 1
                if y_offset < 0:
                    return  # No more space to print messages
