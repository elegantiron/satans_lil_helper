from __future__ import annotations

from input_handlers import BaseInputHandler
from tools import GameWorld

class InGameInputHandler(BaseInputHandler):
    def __init__(self, *, surface, sprites, font=None, world: GameWorld = None):
        super().__init__(surface=surface, sprites=sprites, font=font)
        if world is not None:
            self.world = world
        else:
            self.world = GameWorld()

    def render(self):
        self.world.render(self.surface, self.sprites)