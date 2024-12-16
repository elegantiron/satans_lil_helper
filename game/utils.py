from __future__ import annotations
import hashlib
import hmac
import lzma
import os
from typing import TYPE_CHECKING

import dill as pickle
import numpy as np

from game.constants import HMAC_KEY, Sprites
from game.exceptions import Haxx0red
from game.input_handlers import GameMenuInputHandler

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

def save_data(data, filename):
    if isinstance(data, GameMenuInputHandler):
        data = data._parent
    save_data = lzma.compress(pickle.dumps(data))
    signer = hmac.new(HMAC_KEY, digestmod=hashlib.blake2b)
    signer.update(save_data)
    mac_result = signer.digest()
    with open(filename, "wb") as f:
        f.write(mac_result)
    with open(filename, "ab") as f:
        f.write(save_data)


def load_data(filename):
    signer = hmac.new(HMAC_KEY, digestmod=hashlib.blake2b)
    save_data = ""
    try:
        with open(filename, "rb") as f:
            mac_data = f.read(signer.digest_size)
            save_data = f.read()
        signer.update(save_data)
        computed_mac = signer.digest()
        if computed_mac == mac_data:
            return pickle.loads(lzma.decompress(save_data))
        else:
            raise Haxx0red
    except FileNotFoundError:
        return None