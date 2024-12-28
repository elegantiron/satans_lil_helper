from __future__ import annotations


class PathBlocked(BaseException):
    pass


class InventoryFull(BaseException):
    pass


class MissingComponent(BaseException):
    pass
