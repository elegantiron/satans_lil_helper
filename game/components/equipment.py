from __future__ import annotations

import attrs

from ..constants import EquipmentSlot


@attrs.define
class Equippable:
    slot: EquipmentSlot | None = None
    pdef: int = 0
    mdef: int = 0
    light: int = 0
    damage: tuple[int, int] = 0, 0


@attrs.define
class Equipment:
    slots = {
        EquipmentSlot.Body: True,
        EquipmentSlot.Feet: True,
        EquipmentSlot.Hands: True,
        EquipmentSlot.Head: True,
        EquipmentSlot.Shield: True,
        EquipmentSlot.Weapon: True,
    }
