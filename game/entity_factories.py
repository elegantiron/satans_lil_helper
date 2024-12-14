"""Functions to make entities by adding groups of components and tags."""

from __future__ import annotations
from typing import TYPE_CHECKING


from components import (
    Crit,
    Equipment,
    Evade,
    Health,
    Inventory,
    Level,
    LightRadius,
    Magic,
    MagicDefense,
    Mana,
    Name,
    NextAction,
    PhysicalDefense,
    Position,
    Renderable,
    Strength,
)
from constants import CLASSES, SPRITES, TAGS

if TYPE_CHECKING:
    import tcod.ecs
    import random


def setup_player(entity: tcod.ecs.Entity, player_class: CLASSES) -> None:
    """Adds the necessary components to an entity to make it a player."""
    entity.components[Renderable] = Renderable(SPRITES.PLAYER)
    entity.components[Name] = Name("Player")
    entity.components[NextAction] = NextAction(0)
    entity.components[Inventory] = Inventory(26)
    entity.components[Level] = Level(level=1, xp=0, xp_granted=0)
    entity.components[Renderable] = Renderable(SPRITES.PLAYER)
    entity.components[Position] = Position(0, 0)

    entity.tags.add(TAGS.PLAYER)
    entity.tags.add(TAGS.BLOCKING)
    match player_class:
        case CLASSES.WARRIOR:
            make_warrior(entity)


def make_warrior(entity: tcod.ecs.Entity):
    """Sets up an entity to be a warrior.
    
    :param entity: The entity to make into a warrior"""
    entity.components[Health] = Health(30, 10)
    entity.components[Mana] = Mana(5, 1)
    entity.components[Strength] = Strength(5, 0.5)
    entity.components[Magic] = Magic(0, 0.25)
    entity.components[PhysicalDefense] = PhysicalDefense(7, 0.5)
    entity.components[MagicDefense] = MagicDefense(2, 0.25)
    entity.components[Evade] = Evade(0, 0)
    entity.components[Crit] = Crit(0, 0)
    entity.components[LightRadius] = LightRadius(6, 0)
    entity.components[Equipment] = Equipment(
        one_hand=True,
        mundane=True,
        shield=True,
        body=True,
        head=True,
        hands=True,
        feet=True,
    )

    entity.tags.add(TAGS.WARRIOR)


def make_wolf(entity: tcod.ecs.Entity, rng: random.Random):
    """Sets up an entity to be a hostile wolf.

    :param tcod.ecs.Entity entity: The entity that is assembled into a wolf
    :param random.Random rng: The random number generator to use for the entity's stats

    .. todo::
        - [ ] Add gnaw special attack
        - [ ] Add howl special attack

    """
    entity.components[Health] = Health(rng.randint(1, 8) + 16)
    entity.components[Strength] = Strength(2)
    entity.components[PhysicalDefense] = PhysicalDefense(5)
    entity.components[Crit] = Crit(1)


def make_floor(entity: tcod.ecs.Entity, x: int, y: int):
    """Gives an entity the components and tags to make it a floor tile.
    
    :param entity: Entity to make a floor
    :param x: `x` coordinate of the tile
    :param y: `y` coordinate of the tile
    """
    entity.components[Position] = Position(x, y)
    entity.components[Renderable] = Renderable(SPRITES.FOREST_FLOOR)
    entity.tags.add(TAGS.TILE)

def make_wall(entity: tcod.ecs.Entity, x: int, y: int):
    """Gives an entity the components and tags to make it a wall tile

    :param entity: Entity to make a wall
    :param x: `x` coordinate of the tile
    :param y: `y` coordinate of the tile

    """
    entity.components[Position] = Position(x, y)
    entity.components[Renderable] = Renderable(SPRITES.FOREST_WALL)
    entity.tags.add(TAGS.TILE)
    entity.tags.add(TAGS.BLOCKING)
    entity.tags.add(TAGS.TRANSPARENT)
