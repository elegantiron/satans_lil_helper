from __future__ import annotations

from typing import TYPE_CHECKING

import pytest

from actions import PickupAction
from components import Inventory, Position
from exceptions import Impossible, InventoryFull, MissingComponent, NoItem

if TYPE_CHECKING:
    from tcod.ecs import Entity

    from gameworld import GameWorld


@pytest.mark.depends(on="tests/messagelog_test.py")
class TestPickup:
    def test_pickup_item(self, gameworld_fixed_seed: GameWorld, item: Entity) -> None:
        old_inventory_count = gameworld_fixed_seed.player.components[
            Inventory
        ].item_count
        PickupAction(gameworld_fixed_seed.player).perform()
        assert (
            gameworld_fixed_seed.player.components[Inventory].item_count
            == old_inventory_count + 1
        )

    def test_pickup_no_target(self, gameworld_fixed_seed: GameWorld) -> None:
        with pytest.raises(Impossible) as excinfo:
            PickupAction(gameworld_fixed_seed.player).perform()
        assert excinfo.type is NoItem

    def test_pickup_no_inventory(
        self, gameworld_fixed_seed: GameWorld, entity_no_inventory: Entity
    ) -> None:
        del entity_no_inventory.components[Inventory]
        with pytest.raises(Impossible) as excinfo:
            PickupAction(entity_no_inventory).perform()
        assert excinfo.type is MissingComponent

    def test_pickup_inventory_full(
        self, gameworld_fixed_seed: GameWorld, entity_no_inventory: Entity, item: Entity
    ) -> None:
        with pytest.raises(Impossible) as excinfo:
            PickupAction(entity_no_inventory).perform()
        assert excinfo.type is InventoryFull

    def test_pickup_no_location(
        self, gameworld_fixed_seed: GameWorld, entity_no_inventory: Entity, item: Entity
    ) -> None:
        del entity_no_inventory.components[Position]
        with pytest.raises(Impossible) as excinfo:
            PickupAction(entity_no_inventory).perform()
        assert excinfo.type is MissingComponent
