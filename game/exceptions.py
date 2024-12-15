from __future__ import annotations

class QuitWithoutSaving(SystemExit):
    pass

class GameReset(BaseException):
    pass

class LoadGame(BaseException):
    pass

class MissingComponent(BaseException):
    pass

class InventoryFull(BaseException):
    pass

class Impossible(BaseException):
    pass

class PathBlocked(BaseException):
    pass

class Haxx0red(BaseException):
    pass