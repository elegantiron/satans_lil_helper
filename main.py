from __future__ import annotations

from typing import final

import pygame
import pygame.freetype

from constants import PATHS
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
    for k, v in PATHS:
        img = pygame.image.load(v).convert_alpha()
        sprites.update({k: img})
    handler = MainMenuInputHandler(surface=window)
    
    while running:
        try:
            for event in pygame.event.get():
                handler = handler.handle_events(event)
            window.fill("black")
            handler.render()
            pygame.display.flip()

            clock.tick(60)
        except SystemExit:
            running = False

    pygame.quit()


if __name__ == "__main__":
    main()
