"""Custom exception definitions"""

from __future__ import annotations


class Impossible(BaseException):
    """An entity attempted an impossible action"""


class PathBlocked(Impossible):
    """An entity tried to enter an invalid square"""


class InventoryFull(Impossible):
    """An entity tried to pick up an item, but had no room in its inventory"""


class MissingComponent(BaseException):
    """An entity is missing a necessary component"""


class HashError(BaseException):
    """Raised when a file fails the hash check"""
