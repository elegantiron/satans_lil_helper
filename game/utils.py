from __future__ import annotations
import os
from typing import TYPE_CHECKING

import numpy as np

from game.constants import Sprites

if TYPE_CHECKING:
    import numpy.typing as npt
    import random


def resolve_path(path: str) -> str:
    return os.path.abspath(path)


def new_tile(
    *,
    walkable: int,
    transparent: int,
    sprite_id: Sprites,
    dtype: npt.DTypeLike,
):
    return np.array(
        (walkable, transparent, False, False, False, sprite_id), dtype=dtype
    )


def get_damage_factor(
    *, target_level: int, actor_level: int, rng: random.Random, crit: int
) -> float:
    roll = rng.randint(1, 100)
    if roll < max(0, target_level - actor_level):
        return 0
    elif roll < 10:
        return 0.5
    elif roll < 60:
        return 1
    elif roll < 95 - crit:
        return 1.25
    else:
        return 2


def get_damage(
    *, damage_factor: float, dice: int, sides: int, rng: random.Random
) -> int:
    dmg = 0
    for _ in range(dice):
        dmg += rng.randint(1, sides + 1)
    return int(dmg * damage_factor)
