# nuitka-project: --onefile
# nuitka-project: --follow-imports
# nuitka-project: --include-data-dir=assets=assets
# nuitka-project: --output-filename=slh.exe
### nuitka-project: --windows-console-mode=disable
### nuitka-project: --deployment
# nuitka-project: --output-dir=build


from __future__ import annotations
from pathlib import Path
from typing import TypeVar

import pygame
from pygame.event import Event
import pygame.freetype as freetype

from game.bestiary import Bestiary
from game.constants import GameSettings, Strings
from game.exceptions import GameReset, LoadGame, QuitWithoutSaving
import game.input_handlers as input_handlers
from game.setup import load_fonts, load_sprites
from game.save_funcs import load_data, save_data

Handler = TypeVar("Handler", bound="input_handlers.BaseInputHandler")


def main():
    window = pygame.display.set_mode(GameSettings.WindowSize, display=0, vsync=1)
    working_surface = pygame.Surface(GameSettings.WindowSize)
    pygame.display.set_caption(Strings.Title)
    freetype.init()
    clock = pygame.time.Clock()
    running = True

    sprites = load_sprites()
    fonts = load_fonts()
    bestiary_filepath = Path("./bestiary.dat")
    if bestiary_filepath.exists():
        bestiary = load_data(bestiary_filepath)
    else:
        bestiary = Bestiary()
    handler = input_handlers.MainMenuInputHandler(bestiary=bestiary)
    pygame.event.set_allowed(
        [pygame.QUIT, pygame.KEYDOWN, pygame.MOUSEMOTION, pygame.MOUSEBUTTONDOWN]
    )

    while running:
        """Rendering code"""
        working_surface.fill((0, 0, 0, 0))
        window.fill("black")
        handler.render(
            surface=window,
            working_surface=working_surface,
            sprites=sprites,
            fonts=fonts,
        )
        pygame.display.flip()

        """Event Handling"""
        try:
            for event in pygame.event.get():
                match event:
                    case Event(type=pygame.QUIT):
                        raise SystemExit
                    case Event(type=pygame.KEYDOWN):
                        handler = handler.handle_key(
                            event.key, event.mod, event.unicode, event.scancode
                        )
                    case Event(type=pygame.MOUSEMOTION):
                        handler = handler.handle_mousemotion(
                            event.pos, event.rel, event.buttons, event.touch
                        )
                    case Event(type=pygame.MOUSEBUTTONDOWN):
                        handler = handler.handle_mousebuttondown(
                            event.pos, event.button, event.touch
                        )
        except QuitWithoutSaving:
            running = False
            continue
        except GameReset:
            if isinstance(handler, input_handlers.GameMenuInputHandler):
                handler = handler._parent
            save_data(handler, "./savegame.dat")
            save_data(handler.bestiary, "./bestiary.data")
            handler = input_handlers.MainMenuInputHandler(fonts=fonts)
            continue
        except SystemExit:
            if isinstance(handler, input_handlers.GameMenuInputHandler):
                handler = handler._parent
            save_data(handler, "./savegame.dat")
            save_data(handler.bestiary, "./bestiary.dat")
            running = False
            continue
        except LoadGame:
            handler = load_data("./savegame.dat")
            continue

        clock.tick(144)

    pygame.quit()


if __name__ == "__main__":
    main()
