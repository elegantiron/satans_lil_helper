from os.path import join


class SCREEN:
    PIXEL_WIDTH = 1280
    PIXEL_HEIGHT = 720
    TILE_WIDTH = int(PIXEL_WIDTH / 32)
    TILE_HEIGHT = int(PIXEL_HEIGHT / 32)


class CAMERA:
    WIDTH = int(2 * SCREEN.TILE_WIDTH / 3)
    HEIGHT = int(SCREEN.TILE_HEIGHT)


class STATUS:
    WIDTH = int(SCREEN.PIXEL_WIDTH / 3)
    HEIGHT = SCREEN.PIXEL_HEIGHT


class PATHS:
    FOREST_FLOOR = join("assets", "images", "tiles", "forest", "floor", "000.png")
    FOREST_WALL = join("assets", "images", "tiles", "forest", "wall", "1.png")
    PLAYER = join("assets", "images", "player", "player.png")
    ORC = join("assets", "images", "enemies", "orc.png")
    SACK = join("assets", "images", "items", "sack.png")
    ROBOTO = join("assets", "fonts", "roboto-slab.ttf")
    F25 = join("assets", "fonts", "F25_Bank_Printer.ttf")


class TILES:
    FOREST_FLOOR = 0
    FOREST_WALL = 1
    DOWN_STAIRS = 2


class ENTITIES:
    PLAYER = 0
    ORC = 1
    SACK = 2
    TROLL = PLAYER
    SCROLL = PLAYER
    POTION = PLAYER
    DAGGER = PLAYER
    LEATHER_ARMOR = PLAYER
    CHAIN_MAIL = PLAYER
    SWORD = PLAYER

AREA_NAMES = [
    "Forest"
]