from __future__ import annotations
import lzma
from multiprocessing import Queue
from random import Random

import dill
import pyrotkit

from game.constants import TILE_SIZE
from game.gameworld import GameWorld


def get_new_world(rng: Random, q: Queue, screen_size: tuple[int, int]):
    world = GameWorld(rng=rng, tile_size=TILE_SIZE, screen_size=screen_size)
    print("dumping")
    data = dill.dumps(world)
    print("compressing")
    compressed = lzma.compress(data)
    print("putting in queue")
    q.put(compressed)


def make_new_map(map_generator: pyrotkit.generators.MapGenerator, q: Queue):
    new_map = map_generator.generate_map()
    q.put(lzma.compress(dill.dumps(new_map)))
