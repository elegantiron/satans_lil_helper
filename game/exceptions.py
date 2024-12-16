from __future__ import annotations


class QuitWithoutSaving(SystemExit):
    pass


class GameReset(BaseException):
    pass


class LoadGame(BaseException):
    pass


class Impossible(BaseException):
    """Generic exception indicating that a given action is impossible"""


class PathBlocked(Impossible):
    """Indicates that the movement action can't be completed because the destination square is occupied."""


class Haxx0red(BaseException):
    pass


class InventoryFull(Impossible):
    """Raised when an action can't be completed because the entity's inventory is full"""


class MissingComponent(Impossible):
    """Raised when an action can't be completed because the entity is missing a required component"""
