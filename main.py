# nuitka-project: --onefile
# nuitka-project: --follow-imports
# nuitka-project: --include-data-dir=assets=assets
# nuitka-project: --output-filename=slh.exe
### nuitka-project: --windows-console-mode=disable
### nuitka-project: --deployment
# nuitka-project: --output-dir=build


from __future__ import annotations
from pathlib import Path

import pygame
import pygame.freetype as freetype

from game.bestiary import Bestiary
from game.constants import GameSettings, Strings
from game.exceptions import GameReset, QuitWithoutSaving
from game.setup import load_fonts, load_sprites
from game.save_funcs import load_data
from game.engine import Engine


def main():
    window = pygame.display.set_mode(GameSettings.WindowSize, display=0, vsync=1)
    working_surface = pygame.Surface(GameSettings.WindowSize)
    pygame.display.set_caption(Strings.Title)
    freetype.init()
    clock = pygame.time.Clock()
    running = True

    sprites = load_sprites()
    fonts = load_fonts()
    bestiary_filepath = Path(Strings.BestiaryPath)
    if bestiary_filepath.exists():
        bestiary = load_data(bestiary_filepath)
    else:
        bestiary = Bestiary()
    engine = Engine(bestiary)
    pygame.event.set_allowed(
        [pygame.QUIT, pygame.KEYDOWN, pygame.MOUSEMOTION, pygame.MOUSEBUTTONDOWN]
    )

    while running:
        """Rendering code"""
        working_surface.fill((0, 0, 0, 0))
        window.fill("black")
        engine.render(
            surface=window,
            working_surface=working_surface,
            sprites=sprites,
            fonts=fonts,
        )
        pygame.display.flip()

        """Event Handling"""
        try:
            for event in pygame.event.get():
                engine.dispatch_event(event)
        except QuitWithoutSaving:
            running = False
            continue
        except GameReset:
            raise NotImplementedError
        except SystemExit:
            running = False
            continue

        clock.tick(144)

    pygame.quit()


if __name__ == "__main__":
    main()
