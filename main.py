from __future__ import annotations

from typing import final

import esper
import pygame
import pygame.freetype

from components import Position, Renderable
from constants import PATHS, PRIORITY, SPRITES
from input_handlers import MainMenuInputHandler
from processors import ActionProcessor, RenderProcessor


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
    player = esper.create_entity()
    esper.add_component(player, Position(0, 0))
    esper.add_component(player, Renderable(SPRITES.PLAYER))
    render_processor = RenderProcessor(window, sprites=sprites)
    action_processor = ActionProcessor(player=player)
    handler = MainMenuInputHandler(surface=window)
    esper.add_processor(render_processor, priority=PRIORITY.RENDER)
    esper.add_processor(action_processor, priority=PRIORITY.ACTION)

    while running:
        try:
            for event in pygame.event.get():
                handler = handler.dispatch(event)
            window.fill("black")
            handler.on_draw()
            pygame.display.flip()

            clock.tick(60)
        except SystemExit:
            running = False

    pygame.quit()


if __name__ == "__main__":
    main()
