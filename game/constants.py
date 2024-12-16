from __future__ import annotations
from enum import Enum, IntEnum, StrEnum, auto
from os.path import abspath

import pygame


def resolve_path(path: str) -> str:
    return abspath(path)


TILE_SIZE: int = 32


MOVEMENT_KEYS = {
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
    pygame.K_HOME: (-1, -1),
    pygame.K_END: (-1, 1),
    pygame.K_PAGEUP: (1, -1),
    pygame.K_PAGEDOWN: (1, 1),
}

CONFIRMATION_KEYS = {
    pygame.K_RETURN,
    pygame.K_KP_ENTER,
}


class Sprites(Enum):
    PLAYER = auto()
    FOREST_FLOOR = auto()
    FOREST_WALL = auto()


SPRITEPATHS = {
    Sprites.PLAYER: resolve_path("assets/images/player/player.png"),
    Sprites.FOREST_FLOOR: resolve_path("assets/images/tiles/forest/floor/000.png"),
    Sprites.FOREST_WALL: resolve_path("assets/images/tiles/forest/wall/000.png"),
}


class FontDict(Enum):
    MainMenu = auto()
    GameStatus = auto()
    GameMenu = MainMenu
    # TitleText = auto()
    # ByLineText = auto()


FONT_SETTINGS = {
    FontDict.MainMenu: (
        resolve_path("assets/foNts/F25_Bank_Printer.ttf"),
        20,
        (0xFF, 0xFF, 0xFF, 0xFF),
        (0x00, 0x00, 0x00, 0x00),
    ),
    FontDict.GameStatus: (
        resolve_path("assets/fonts/F25_Bank_Printer.ttf"),
        15,
        (0xFF, 0xFF, 0xFF, 0xFF),
        (0x00, 0x00, 0x00, 0x00),
    ),
}


class Strings(StrEnum):
    New_Game = "New Game"
    QuitToDesktop = "Quit to Desktop"
    LoadGame = "Continue"
    Bestiary = "Bestiary"
    QuitToMenu = "Main Menu"
    QuitWithSave = "Save and Quit"
    QuitNoSave = "Quit without Saving"
    Resume = "Resume"


HMAC_KEY = b"special_key_for_slh"


class Durations(IntEnum):
    PlayerMovement = 5


class Professions(StrEnum):
    Warrior = "Warrior"


class Tile(StrEnum):
    Walkable = "walkable"
    Transparent = "transparent"
    Explored = "explored"
    Safe = "Safe"
    Visible = "Visible"
    SpriteID = "sprite_id"


class EnemyType(StrEnum):
    Wolf = "Wolf"


class EnemyDescriptions:
    EnemyType.Wolf


class Tags(Enum):
    Friendly = auto()
    Hostile = auto()
    Equipped = auto()
    Item = auto()
    Blocking = auto()
    Player = auto()
    Warrior = auto()
    HeldBy = auto()
    Holding = auto()
    Transparent = auto()
