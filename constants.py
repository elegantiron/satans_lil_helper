from __future__ import annotations
from enum import Enum, Flag, IntEnum, auto
from os.path import join

import pygame


DIRS = [(-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1)]

class EQUIPMENT(Flag):
    HEAD = auto()
    MUNDANE = auto()
    MAGICAL = auto()
    ONE_HAND = auto()
    TWO_HAND = auto()
    WAND = auto()
    SECONDARY_WEAPON = auto()
    SECONDARY_SHIELD = auto()
    BODY = auto()
    HANDS = auto()
    FEET = auto()
    LEGS = auto()

class KEYS:
    MOVEMENT = {
        pygame.K_KP1: (-1, 1),
        pygame.K_KP2: (0, 1),
        pygame.K_KP3: (1, 1),
        pygame.K_KP4: (-1, 0),
        pygame.K_KP6: (1, 0),
        pygame.K_KP7: (-1, -1),
        pygame.K_KP8: (0, -1),
        pygame.K_KP9: (1, -1),
        pygame.K_UP: (0, -1),
        pygame.K_DOWN: (0, 1),
        pygame.K_RIGHT: (1, 0),
        pygame.K_LEFT: (-1, 0),
    }
    CONFIRMATION = {
        pygame.K_RETURN,
        pygame.K_KP_ENTER,
    }


class PRIORITY(IntEnum):
    """Priority order for esper processing."""
    # esper executes processors in order of priority, starting at the highest and proceeding toward 0
    RENDER = auto()
    ACTION = auto()


class SPRITES(Enum):
    FOREST_FLOOR = auto()
    FOREST_WALL = auto()
    FOREST_ORC = auto()
    PLAYER = auto()
    SACK = auto()


PATHS = {
    (
        SPRITES.FOREST_FLOOR,
        join("assets", "images", "tiles", "forest", "floor", "000.png"),
    ),
    (
        SPRITES.FOREST_WALL,
        join("assets", "images", "tiles", "forest", "wall", "000.png"),
    ),
    (SPRITES.FOREST_ORC, join("assets", "images", "enemies", "orc.png")),
    (SPRITES.PLAYER, join("assets", "images", "player", "player.png")),
}

TILE_SIZE = 32
