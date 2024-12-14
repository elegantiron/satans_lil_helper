"""Constants used throughout the app.

There should only be one source of truth!"""

from __future__ import annotations
from enum import Enum, Flag, auto
import os
import sys

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
    SHIELD = auto()
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


class SPRITES(Enum):
    FOREST_FLOOR = auto()
    FOREST_WALL = auto()
    FOREST_ORC = auto()
    PLAYER = auto()
    SACK = auto()

def resource_path(relative_path):
    """Get absolute path to resource."""
    try:
        base_path = sys._MEIPASS
    except Exception:
        base_path = os.path.abspath('.')

    return os.path.join(base_path, relative_path)

SPRITEPATHS = {
    (SPRITES.FOREST_FLOOR, resource_path("assets/images/tiles/forest/floor/000.png")),
    (SPRITES.FOREST_WALL, resource_path("assets/images/tiles/forest/wall/000.png")),
    (SPRITES.FOREST_ORC, resource_path("assets/images/enemies/orc.png")),
    (SPRITES.PLAYER, resource_path("assets/images/player/player.png")),
    
}

PATHS = {
    "F25": resource_path("assets/fonts/F25_Bank_Printer.ttf")
}
TILE_SIZE = 32


class CLASSES(Enum):
    WARRIOR = auto()


class TAGS(Enum):
    FRIENDLY = auto()
    HOSTILE = auto()
    EQUIPPED = auto()
    ITEM = auto()
    BLOCKING = auto()
    PLAYER = auto()
    WARRIOR = auto()
    HELD_BY = auto()
    HOLDING = auto()
    TILE = auto()
    TRANSPARENT = auto()


class STRINGS:
    NEW_GAME = "New Game"
    EXIT = "Exit"
    TITLE = "Satan's Lil Helper"
    CLASSES = {CLASSES.WARRIOR: "Warrior"}
    CONTINUE = "Continue"
    EXIT_TO_MENU = "Exit to Main Menu"
    EXIT_TO_DESKTOP = "Exit to Desktop"


class MAPS:
    FOREST_WIDTH = 100
    FOREST_HEIGHT = 100

class TILE:
    WALKABLE = "walkable"
    TRANSPARENT = "transparent"
    EXPLORED = "explored"
    SPRITE = "sprite"