from __future__ import annotations

from typing import TYPE_CHECKING

import attrs

if TYPE_CHECKING:
    from constants import ItemType, abilities


@attrs.define
class Item:
    type: ItemType


@attrs.define
class Equippable(Item):
    requirements: abilities.Abilities


@attrs.define
class Consumable(Item):
    pass
