import traceback

import pygame
import pygame.freetype

# import color
# import exceptions
import setup_game
import input_handlers

from constants import SCREEN, PATHS


def save_game(handler: input_handlers.BaseEventHandler, filename: str) -> None:
    """If the current event handler has an active Engine, then save it."""
    if isinstance(handler, input_handlers.EventHandler):
        handler.engine.save_as(filename)
        print("Game saved.")


def main() -> None:
    pygame.display.init()
    pygame.font.init()
    pygame.freetype.init()

    handler: input_handlers.BaseEventHandler = setup_game.MainMenu()

    flags = pygame.SRCALPHA
    surface = pygame.display.set_mode(
        (SCREEN.PIXEL_WIDTH, SCREEN.PIXEL_HEIGHT), flags=flags
    )
    clock = pygame.time.Clock()
    tile_sprites = [
        pygame.image.load(PATHS.FOREST_FLOOR).convert(),
        pygame.image.load(PATHS.FOREST_WALL).convert(),
    ]
    entity_sprites = [
        pygame.image.load(PATHS.PLAYER).convert_alpha(),
        pygame.image.load(PATHS.ORC).convert_alpha(),
        pygame.image.load(PATHS.SACK).convert_alpha(),
    ]
    f25 = pygame.freetype.Font(file=PATHS.F25, size=15)
    running = True

    # try:
    while running:
        surface.fill("black")
        handler.on_render(
            surface=surface,
            tile_sprites=tile_sprites,
            entity_sprites=entity_sprites,
            font=f25,
        )
        pygame.display.flip()

        try:
            for event in pygame.event.get(
                eventtype=[
                    pygame.KEYDOWN,
                    pygame.MOUSEMOTION,
                    pygame.MOUSEBUTTONDOWN,
                    pygame.QUIT,
                ]
            ):
                handler = handler.handle_events(event)
        except Exception:  # Handle exceptions in game
            traceback.print_exc()  # Print error to stderr
            # Then print the error to the message log.
            #     if isinstance(handler, input_handlers.EventHandler):
            #         handler.engine.message_log.add_message(
            #             traceback.format_exc(), color.error
            #         )
        clock.tick()
    # except exceptions.QuitWithoutSaving:
    #     running = False
    # except SystemExit:  # Save and exit
    #     save_game(handler, "savegame.sav")
    #     running = False
    # except BaseException:  # Save on any other unexpected exception.
    #     save_game(handler, "savegame.sav")
    #     running = False


if __name__ == "__main__":
    main()
