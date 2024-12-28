from __future__ import annotations


class Impossible(BaseException):
    pass


class PathBlocked(Impossible):
    pass


class InventoryFull(Impossible):
    pass


class MissingComponent(BaseException):
    pass
