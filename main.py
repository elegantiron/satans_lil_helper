"""Satan's Lil Helper"""

from __future__ import annotations

from typing import final

import pygame
import pygame.freetype

from constants import SPRITEPATHS
from input_handlers import MainMenuInputHandler


def main():
    WINDOW_SIZE: final = (1280, 720)

    # pygame initialization
    pygame.display.init()
    pygame.freetype.init()
    pygame.event.set_allowed(
        [
            pygame.KEYDOWN,
            pygame.QUIT,
            pygame.MOUSEBUTTONDOWN,
            pygame.MOUSEMOTION,
            pygame.MOUSEWHEEL,
        ]
    )

    clock = pygame.time.Clock()
    window = pygame.display.set_mode(WINDOW_SIZE)
    running = True
    sprites = {}
    for k, v in SPRITEPATHS:
        img = pygame.image.load(v).convert_alpha()
        sprites.update({k: img})
    handler = MainMenuInputHandler(surface=window, sprites=sprites)
    font = pygame.freetype.Font("assets/fonts/F25_Bank_Printer.ttf", 15)
    while running:
        try:
            for event in pygame.event.get():
                handler = handler.handle_events(event)
            window.fill("black")
            handler.render()
            font.render_to(
                surf=window,
                text=f"{clock.get_fps()}",
                dest=(0, 0),
                fgcolor=(0xFF, 0xFF, 0xFF, 0xFF),
                bgcolor=(0, 0, 0, 0xC0),
            )
            pygame.display.flip()

            clock.tick()
        except SystemExit:
            running = False

    pygame.quit()


if __name__ == "__main__":
    main()
