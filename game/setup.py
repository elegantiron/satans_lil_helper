from __future__ import annotations

from pygame import Surface, image
from pygame.freetype import Font

from game.constants import FontDict, FONT_SETTINGS, Sprites, SPRITEPATHS
from game.utils import resolve_path


def load_sprites() -> dict[Sprites, Surface]:
    data = {}
    for label in Sprites:
        if label in SPRITEPATHS.keys():
            data.update(
                {label: image.load(resolve_path(SPRITEPATHS[label])).convert_alpha()}
            )
    fog_surface = Surface((32, 32))
    fog_surface.fill((0x00, 0x00, 0x00))
    fog_surface.set_alpha(0x50)
    data.update({Sprites.FogTile: fog_surface})

    return data


def load_fonts() -> dict[FontDict, Font]:
    data: dict[FontDict, Font] = {}
    for label in FontDict:
        path, size, fgcolor, bgcolor = FONT_SETTINGS[label]
        data.update({label: Font(resolve_path(path), size)})
        if fgcolor is not None:
            data[label].fgcolor = fgcolor
        if bgcolor is not None:
            data[label].bgcolor = bgcolor

    return data
