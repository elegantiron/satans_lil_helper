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
        BESTIARY = "bestiary"
        LEVEL_UP = "level up"
        INVENTORY = "inventory"
        PAUSE = "pause"
        GAME = "Satan's Lil Helper"
        MAIN_MENU = GAME
        STATUS = "status"
        CHARACTER_SHEET = "character info"

    class Status(StrEnum):
        LOCATION = "location"
        HEALTH = "health"
        MANA = "mana"
        ABILITIES = "abilities"
        STRENGTH = "strength"
        MAGIC = "magic"
        EVASION = "evasion"
        CRIT = "crit"
        EQUIPMENT = "equipment"
        LIGHT = "light radius"
        VISION = "vision radius"
        SKILLS = "skills"

    class GearSlots(StrEnum):
        HEAD = "helm"
        BODY = "armor"
        HANDS = "gauntlets"
        FEET = "boots"
        WEAPON = "weapon"
        
