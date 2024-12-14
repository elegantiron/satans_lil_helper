from __future__ import annotations
import random

import numpy as np
from pygame import Surface

from game.constants import SPRITES, TILE


def get_damage_factor(
    *, target_level: int, actor_level: int, rng: random.Random, crit: int
) -> float:
    """Calculates a damage factor.

    :param target_level: Level of the target entity
    :param actor_level: Level of the acting entity
    :param crit: crit stat of the attacker
    :return: Damage factor for the attack
    """
    if roll := rng.randint(1, 100) < max(0, target_level - actor_level):
        return 0
    elif roll < 10:
        return 0.5
    elif roll < 60:
        return 1
    elif roll < 95 - crit:
        return 1.25
    else:
        return 2


def get_damage(*, damage_factor: float, dice: int, sides: int) -> int:
    """Calculates the damage for an arbitrary attack.

    :param damage_factor: The attack's damage factor
    :param dice: The weapon's die count
    :param sides: The number of sides per die
    :return: Damage for the attack

    .. todo::
       - [ ] Implement damage calculator"""
    pass


def get_shade_surface(*, dims: tuple[int, int]) -> Surface:
    surface = Surface(dims)
    surface.set_alpha(0x50)
    surface.fill("black")
    return surface


tile_dt = np.dtype(
    [
        (TILE.WALKABLE, bool),
        (TILE.TRANSPARENT, bool),
        (TILE.EXPLORED, bool),
        (TILE.SPRITE, SPRITES),
    ]
)


def new_tile(
    *,
    walkable: bool,
    transparent: bool,
    sprite_id: SPRITES,
):
    return np.array((walkable, transparent, False, sprite_id), dtype=tile_dt)
