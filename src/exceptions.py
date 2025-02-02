"""Custom exception definitions"""

from __future__ import annotations


class Impossible(BaseException):
    """An entity attempted an impossible action"""


class PathBlocked(Impossible):
    """An entity tried to enter an invalid square"""


class InventoryFull(Impossible):
    """An entity tried to pick up an item, but had no room in its inventory"""


class MissingComponent(Impossible):
    """An entity is missing a necessary component"""


class HashError(BaseException):
    """Raised when a file fails the hash check"""


class SpawnBlocked(Impossible):
    """Raised when an entity is spawned in a wall"""


class OutOfBounds(Impossible):
    """Raised when an action targets something outside the world."""

class NoItem(Impossible):
    """Raised when trying to pick up an item where none exists"""