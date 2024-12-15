from __future__ import annotations

from pygame import Surface, image
from pygame.freetype import Font

from game.constants import FontDict, FONT_SETTINGS, Sprites, SPRITEPATHS

def load_sprites() -> dict[Sprites, Surface]:
    data = {}
    for label in Sprites:
        data.update({label: image.load(SPRITEPATHS[label]).convert_alpha()})

    return data

def load_fonts() -> dict[FontDict, Font]:
    data: dict[FontDict, Font] = {}
    for label in FontDict:
        path, size, fgcolor, bgcolor = FONT_SETTINGS[label]
        data.update({label: Font(path, size)})
        if fgcolor is not None:
            data[label].fgcolor=fgcolor
        if bgcolor is not None:
            data[label].bgcolor=bgcolor

    return data