# nuitka-project: --onefile
# nuitka-project: --follow-imports
# nuitka-project: --include-data-dir=assets=assets
# nuitka-project: --output-filename=slh.exe
# nuitka-project: --windows-console-mode=disable
# nuitka-project: --deployment


from __future__ import annotations
from pathlib import Path
from typing import TypeVar

import pygame
import pygame.freetype as freetype

from game.bestiary import Bestiary
from game.exceptions import GameReset, LoadGame, QuitWithoutSaving
from game.input_handlers import (
    BaseInputHandler,
    GameMenuInputHandler,
    MainMenuInputHandler,
)
from game.setup import load_fonts, load_sprites
from game.utils import load_data, save_data

Handler = TypeVar("Handler", bound="BaseInputHandler")





def main():
    window = pygame.display.set_mode((1280, 720), display=0, vsync=1)
    pygame.display.set_caption("Satan's Lil Helper")
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
    handler = MainMenuInputHandler(bestiary=bestiary)

    while running:
        window.fill("black")
        handler.render(surface=window, sprites=sprites, fonts=fonts)
        # fonts[FontDict.GameStatus].render_to(window, (0, 0), text=str(clock.get_fps()))
        pygame.display.flip()
        try:
            for event in pygame.event.get(
                [
                    pygame.QUIT,
                    pygame.KEYDOWN,
                    pygame.MOUSEMOTION,
                    pygame.MOUSEBUTTONDOWN,
                ]
            ):
                match event.type:
                    case pygame.QUIT:
                        raise SystemExit
                    case pygame.KEYDOWN:
                        handler = handler.handle_key(
                            event.key, event.mod, event.unicode, event.scancode
                        )
                    case pygame.MOUSEMOTION:
                        handler = handler.handle_mousemotion(
                            event.pos, event.rel, event.buttons, event.touch
                        )
        except QuitWithoutSaving:
            running = False
            continue
        except GameReset:
            if isinstance(handler, GameMenuInputHandler):
                handler = handler._parent
            save_data(handler, "./savegame.dat")
            save_data(handler.bestiary, "./bestiary.data")
            handler = MainMenuInputHandler(fonts=fonts)
            continue
        except SystemExit:
            if isinstance(handler, GameMenuInputHandler):
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
