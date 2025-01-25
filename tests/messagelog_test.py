from __future__ import annotations

import pytest

from constants import Colors
from messagelog import MessageLog


@pytest.fixture(scope="class")
def messagelog() -> MessageLog:
    return MessageLog()


@pytest.mark.parametrize(
    ("text", "color"),
    [
        ("You attack the goblin dealing a bunch of emotional damage", Colors.PlayerAttack),
        ("The hipster attacks you, but your confidence negates his disdain!", Colors.EnemyAttack),
        ("A third event happens, but it doesn't go your way.", Colors.AmericanRose),
    ],
)
class TestMessageLog:
    def test_logging(
        self, messagelog: MessageLog, text: str, color: tuple[int, int, int, int]
    ) -> None:
        messagelog.add_message(text, color)
