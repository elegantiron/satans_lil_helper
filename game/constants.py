from __future__ import annotations

from enum import Enum, IntEnum, StrEnum, auto
from os.path import join
from typing import TYPE_CHECKING

import numpy as np
import pygame

from game import colors

if TYPE_CHECKING:
    import numpy.typing as npt


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
    Player = auto()
    ForestFloor = auto()
    ForestWall = auto()
    FogTile = auto()


SPRITEPATHS = {
    Sprites.Player: join("assets", "images", "player", "player.png"),
    Sprites.ForestFloor: join(
        "assets", "images", "tiles", "forest", "floor", "000.png"
    ),
    Sprites.ForestWall: join("assets", "images", "tiles", "forest", "wall", "000.png"),
}


class FontDict(Enum):
    MainMenu = auto()
    GameStatus = auto()
    GameMenu = MainMenu
    ClassName = auto()
    TitleText = auto()
    # ByLineText = auto()
    ChooseClass = auto()


FONT_SETTINGS = {
    FontDict.MainMenu: (
        "assets/fonts/F25_Bank_Printer.ttf",
        20,
        (0xFF, 0xFF, 0xFF, 0xFF),
        (0x00, 0x00, 0x00, 0x00),
    ),
    FontDict.GameStatus: (
        "assets/fonts/F25_Bank_Printer.ttf",
        15,
        (0xFF, 0xFF, 0xFF, 0xFF),
        (0x00, 0x00, 0x00, 0x00),
    ),
    FontDict.ClassName: (
        "assets/fonts/FairyDustB.ttf",
        35,
        colors.White,
        colors.Transparent,
    ),
    FontDict.TitleText: (
        "assets/fonts/FairyDustB.ttf",
        75,
        colors.Title,
        colors.Transparent,
    ),
    FontDict.ChooseClass: (
        "assets/fonts/FairyDustB.ttf",
        75,
        colors.ChooseClass,
        colors.Transparent,
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
    ChoosePlayerClass = "Choose Your Class"
    Title = "Satan's Lil Helper"
    PathBlocked = "The way is blocked."
    BestiaryPath = "./bestiary.dat"
    WorldPath = "./game.dat"
    Status = "Status"


HMAC_KEY = b"special_key_for_slh"


class Durations(IntEnum):
    PlayerMovement = 5


class TileDict(StrEnum):
    Walkable = "walkable"
    Transparent = "transparent"
    Explored = "explored"
    Safe = "Safe"
    Visible = "Visible"
    SpriteID = "sprite_id"
    MovementCost = "movement_cost"


class EnemyType(StrEnum):
    Wolf = "Wolf"


class BestiaryDescriptions:
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


class Generators(Enum):
    EarlyForest = auto()


tile_dt = np.dtype(
    [
        (TileDict.Walkable, bool),
        (TileDict.Transparent, bool),
        (TileDict.Explored, bool),
        (TileDict.Visible, bool),
        (TileDict.Safe, bool),
        (TileDict.MovementCost, int),
        (TileDict.SpriteID, Sprites),
    ]
)


def new_tile(
    *,
    walkable: int,
    transparent: int,
    sprite_id: Sprites,
    dtype: npt.DTypeLike,
    movement_cost: int,
):
    return np.array(
        (walkable, transparent, False, False, False, movement_cost, sprite_id),
        dtype=dtype,
    )


class Forest:
    Width = 400
    Height = 400
    EarlyProb = 0.38
    Floor = new_tile(
        walkable=True,
        transparent=True,
        sprite_id=Sprites.ForestFloor,
        dtype=tile_dt,
        movement_cost=1,
    )

    Wall = new_tile(
        walkable=False,
        transparent=False,
        sprite_id=Sprites.ForestWall,
        dtype=tile_dt,
        movement_cost=0,
    )


class Status(Enum):
    Hostile = auto()
    Confused = auto()
    Wandering = auto()


DIRS = [(x, y) for x in range(-1, 2) for y in range(-1, 2) if (x, y) != (0, 0)]


class GameSettings:
    WindowSize = (1280, 720)


class Profession(Enum):
    Warrior = auto()
    Thief = auto()
    Wizard = auto()


ProfessionDescriptions = [
    [
        Profession.Warrior,
        "Warrior",
        [
            "A basic warrior class.",
            "",
            "Proficient with one handed weapons, shields, and all",
            "armor types.",
        ],
    ],
    # [
    #     Professions.Thief,
    #     "Thief",
    #     [
    #         "A basic thief class.",
    #         "",
    #         "Proficient with one handed weapons and light armor.",
    #     ],
    # ],
    # [
    #     Professions.Wizard,
    #     "Wizard",
    #     ["A basic wizard class.", "", "Proficient with magic weapons."],
    # ],
]


class HandlerActions(Enum):
    ShowBestiary = auto()
    ShowMainMenu = auto()
    ShowGameMenu = auto()
    ShowClassSelect = auto()
    ShowPrevious = auto()
    ShowParent = auto()
    ShowGame = auto()
    Noop = auto()
    LoadGame = auto()
    NewGame = auto()
    SaveAndQuit = auto()

class EquipmentSlot(Enum):
    Weapon = auto()
    Shield = auto()
    Head = auto()
    Feet = auto()
    Hands = auto()
    Body = auto()
    Mundane = auto()
    Magic = auto()