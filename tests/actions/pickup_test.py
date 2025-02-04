from __future__ import annotations

from collections.abc import Iterator
from typing import TYPE_CHECKING, Final

import pytest
from tcod.ecs.entity import Entity

from actions import PickupAction
from components import Inventory, Position
from entities import consumables
from exceptions import Impossible, InventoryFull, MissingComponent, NoItem
from gameworld import GameWorld
from messagelog import MessageLog

if TYPE_CHECKING:
    from collections.abc import Iterator

    from tcod.ecs import Entity

SEED: Final[float] = 1737855529.0953882


@pytest.fixture(scope="class")
def gameworld() -> GameWorld:
    return GameWorld(SEED)


@pytest.fixture
def message_log() -> MessageLog:
    return MessageLog()


@pytest.fixture
def item(gameworld: GameWorld) -> Entity:
    return gameworld.spawn_entity(
        consumables.health_potion,
        (
            gameworld.player.components[Position].x,
            gameworld.player.components[Position].y,
        ),
    )


@pytest.fixture
def entity(gameworld: GameWorld) -> Iterator[Entity]:
    new_entity = gameworld.registry.new_entity()
    new_entity.components[Position] = Position(
        x=gameworld.player.components[Position].x,
        y=gameworld.player.components[Position].y,
    )
    new_entity.components[Inventory] = Inventory()
    yield new_entity
    new_entity.clear()


@pytest.mark.depends(on="tests/messagelog_test.py")
class TestPickup:
    def test_pickup_item(self, gameworld: GameWorld, item: Entity) -> None:
        old_inventory_count = gameworld.player.components[Inventory].item_count
        PickupAction(gameworld.player).perform()
        assert (
            gameworld.player.components[Inventory].item_count == old_inventory_count + 1
        )

    def test_pickup_no_target(self, gameworld: GameWorld) -> None:
        with pytest.raises(Impossible) as excinfo:
            PickupAction(gameworld.player).perform()
        assert excinfo.type is NoItem

    def test_pickup_no_inventory(self, gameworld: GameWorld, entity: Entity) -> None:
        del entity.components[Inventory]
        with pytest.raises(Impossible) as excinfo:
            PickupAction(entity).perform()
        assert excinfo.type is MissingComponent

    def test_pickup_inventory_full(
        self, gameworld: GameWorld, entity: Entity, item: Entity
    ) -> None:
        with pytest.raises(Impossible) as excinfo:
            PickupAction(entity).perform()
        assert excinfo.type is InventoryFull

    def test_pickup_no_location(
        self, gameworld: GameWorld, entity: Entity, item: Entity
    ) -> None:
        del entity.components[Position]
        with pytest.raises(Impossible) as excinfo:
            PickupAction(entity).perform()
        assert excinfo.type is MissingComponent
