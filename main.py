# nuitka-project: --onefile
# nuitka-project: --follow-imports
# nuitka-project: --include-data-dir=assets=assets
# nuitka-project: --output-filename=slh.exe
# nuitka-project: --windows-console-mode=disable
# nuitka-project: --deployment


from __future__ import annotations
import hashlib
import hmac
import lzma
from pathlib import Path
import pickle
from typing import TypeVar

import pygame
import pygame.freetype as freetype

from game.bestiary import Bestiary
from game.constants import HMAC_KEY
from game.exceptions import GameReset, Haxx0red, LoadGame, QuitWithoutSaving
from game.input_handlers import (
    BaseInputHandler,
    GameMenuInputHandler,
    MainMenuInputHandler,
)
from game.setup import load_fonts, load_sprites

Handler = TypeVar("Handler", bound="BaseInputHandler")


def save_data(data, filename):
    if isinstance(data, GameMenuInputHandler):
        data = data._parent
    save_data = lzma.compress(pickle.dumps(data))
    signer = hmac.new(HMAC_KEY, digestmod=hashlib.blake2b)
    signer.update(save_data)
    mac_result = signer.digest()
    with open(filename, "wb") as f:
        f.write(mac_result)
    with open(filename, "ab") as f:
        f.write(save_data)


def load_data(filename):
    signer = hmac.new(HMAC_KEY, digestmod=hashlib.blake2b)
    save_data = ""
    try:
        with open(filename, "rb") as f:
            mac_data = f.read(signer.digest_size)
            save_data = f.read()
        signer.update(save_data)
        computed_mac = signer.digest()
        if computed_mac == mac_data:
            return pickle.loads(lzma.decompress(save_data))
        else:
            raise Haxx0red
    except FileNotFoundError:
        return None


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
