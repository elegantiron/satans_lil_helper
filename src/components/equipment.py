"""Equipment definitions"""
from __future__ import annotations

import attrs
from constants import ItemType


@attrs.define(kw_only=True)
class Equipment:
    """Tracks what equipment an entity can use"""
    one_hand: bool = False
    two_hand: bool = False
    mundane: bool = False
    magical: bool = False
    secondary_weapon: bool = False
    shield: bool = False
    body_armor: bool = False
    gauntlets: bool = False
    boots: bool = False
    greaves: bool = False
    helmet: bool = False

    def usable(self, item_type: ItemType) -> bool:
        """Check whether or not an item class is usable"""
        match item_type:
            case ItemType.ONE_HAND:
                return self.one_hand
            case ItemType.TWO_HAND:
                return self.two_hand
            case ItemType.MUNDANE:
                return self.mundane
            case ItemType.MAGICAL:
                return self.magical
            case ItemType.SECONDARY:
                return self.secondary_weapon
            case ItemType.SHIELD:
                return self.shield
            case ItemType.BODY_ARMOR:
                return self.body_armor
            case ItemType.GAUNTLETS:
                return self.gauntlets
            case ItemType.BOOTS:
                return self.boots
            case ItemType.GREAVES:
                return self.greaves
            case ItemType.HELMET:
                return self.helmet
