from __future__ import annotations

from pathlib import Path
from typing import TYPE_CHECKING, Final

import arcade
import pytest

from components import Inventory, Position
from entities import consumables, enemies
from gameworld import GameWorld
from messagelog import MessageLog

if TYPE_CHECKING:
    from collections.abc import Iterator

    from tcod.ecs import Entity

# pylint: disable=redefined-outer-name

path = Path(__file__).parent.parent.resolve()
arcade.resources.add_resource_handle("images", Path(path) / "assets" / "images")

SEED: Final[float] = 1737855529.0953882


@pytest.fixture
def message_log() -> MessageLog:
    return MessageLog()


@pytest.fixture
def item(gameworld_fixed_seed: GameWorld) -> Entity:
    return gameworld_fixed_seed.spawn_entity(
        consumables.health_potion,
        (
            gameworld_fixed_seed.player.components[Position].x,
            gameworld_fixed_seed.player.components[Position].y,
        ),
    )


@pytest.fixture
def entity_no_inventory(gameworld_fixed_seed: GameWorld) -> Iterator[Entity]:
    new_entity = gameworld_fixed_seed.registry.new_entity()
    new_entity.components[Position] = Position(
        x=gameworld_fixed_seed.player.components[Position].x,
        y=gameworld_fixed_seed.player.components[Position].y,
    )
    new_entity.components[Inventory] = Inventory()
    yield new_entity
    new_entity.clear()


@pytest.fixture(scope="class")
def gameworld_fixed_seed() -> GameWorld:
    return GameWorld(SEED)


@pytest.fixture
def wolf_one_below(gameworld_fixed_seed: GameWorld) -> Iterator[Entity]:
    nentity = gameworld_fixed_seed.spawn_entity(
        enemies.forest.wolf,
        (
            gameworld_fixed_seed.player.components[Position].x,
            gameworld_fixed_seed.player.components[Position].y - 1,
        ),
    )
    yield nentity
    nentity.clear()


@pytest.fixture
def wolf_one_above(gameworld_fixed_seed: GameWorld) -> Iterator[Entity]:
    nentity = gameworld_fixed_seed.spawn_entity(
        enemies.forest.wolf,
        (
            gameworld_fixed_seed.player.components[Position].x,
            gameworld_fixed_seed.player.components[Position].y + 1,
        ),
    )
    yield nentity
    nentity.clear()


@pytest.fixture
def strong_entity(gameworld_fixed_seed: GameWorld) -> Iterator[Entity]:
    sentity = gameworld_fixed_seed.spawn_entity(
        enemies.testing.strong,
        (
            gameworld_fixed_seed.player.components[Position].x,
            gameworld_fixed_seed.player.components[Position].y - 1,
        ),
    )
    yield sentity
    sentity.clear()


@pytest.fixture
def weak_entity(gameworld_fixed_seed: GameWorld) -> Entity:  # type: ignore
    wentity = gameworld_fixed_seed.spawn_entity(
        enemies.testing.weak,
        (
            gameworld_fixed_seed.player.components[Position].x,
            gameworld_fixed_seed.player.components[Position].y - 1,
        ),
    )
    yield wentity  # type: ignore
    wentity.clear()
