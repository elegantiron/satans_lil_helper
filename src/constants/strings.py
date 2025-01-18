"""Strings constant"""

from __future__ import annotations

from enum import StrEnum


class Strings:
    """Strings definitions"""

    class Misc(StrEnum):
        PRESS_START = "Press any key to continue"

    class Menu(StrEnum):
        NEW_GAME = "New Game"
        RESUME = "Resume"
        QUIT_TO_DESKTOP = "Quit to Desktop"
        SAVE_AND_QUIT = "Save and Quit"
        QUIT_NO_SAVE = "Quit without Saving"

    class Titles(StrEnum):
        BESTIARY = "Bestiary"
        LEVEL_UP = "Level Up"
        INVENTORY = "Inventory"
        PAUSE = "Pause"
        GAME = "Satan's Lil Helper"
        MAIN_MENU = GAME
        STATUS = "Status"

    class Status(StrEnum):
        LOCATION = "Location"
        HEALTH = "Health: "
        MANA = "Mana: "