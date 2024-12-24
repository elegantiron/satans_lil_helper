from __future__ import annotations
import os
from typing import TYPE_CHECKING
import sys


if TYPE_CHECKING:
    import random
    import pygame
    import pygame.freetype


def resolve_path(path: str) -> str:
    try:
        base_path = sys._MEIPASS
    except Exception:
        base_path = os.path.abspath(".")
    return os.path.join(base_path, path)


def get_damage_factor(
    *, target_level: int, actor_level: int, rng: random.Random, crit: int
) -> float:
    roll = rng.randint(1, 100)
    if roll <= max(0, target_level - actor_level) or roll == 1:
        return 0
    elif roll >= 95 - crit:
        return 2
    elif roll <= 10:
        return 0.5
    elif roll <= 60:
        return 1
    else:
        return 1.25


def get_damage(
    *, damage_factor: float, dice: int, sides: int, rng: random.Random
) -> int:
    dmg = 0
    for _ in range(dice):
        dmg += rng.randint(1, sides)
    return int(dmg * damage_factor)


def write_centered(
    *, surface: pygame.Surface, font: pygame.freetype.Font, text: str, rect: pygame.Rect
) -> pygame.Rect:
    temp_rect = font.get_rect(text=text)
    temp_rect.left = rect.left + ((rect.w - temp_rect.w) // 2)
    font.render_to(surf=surface, dest=temp_rect, text=None)
    return temp_rect
