from __future__ import annotations

from input_handlers import MainGameInputHandler
from tools import get_shade_surface

class DialogInputHandler(MainGameInputHandler):
    """Handles showing the player an NPC's dialog.

    .. todo::
       - [ ] Implement drawing the dialog
       - [ ] Implement advancing through dialog"""

    def __init__(self, *, surface, sprites, font=None):
        super().__init__(surface=surface, font=font, sprites=sprites)
        rect = self.surface.get_rect()
        self.shade = get_shade_surface((rect.w, rect.h))

    def render() -> None:
        super().render()