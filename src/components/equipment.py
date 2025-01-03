from __future__ import annotations

import attrs
from constants import ItemType


@attrs.define(kw_only=True)
class Equipment:
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

    def usable(self, type: ItemType) -> bool:
        match type:
            case ItemType.OneHand:
                return self.one_hand
            case ItemType.TwoHand:
                return self.two_hand
            case ItemType.Mundane:
                return self.mundane
            case ItemType.Magical:
                return self.magical
            case ItemType.SecondaryWeapon:
                return self.secondary_weapon
            case ItemType.Shield:
                return self.shield
            case ItemType.BodyArmor:
                return self.body_armor
            case ItemType.Gauntlets:
                return self.gauntlets
            case ItemType.Boots:
                return self.boots
            case ItemType.Greaves:
                return self.greaves
            case ItemType.Helmet:
                return self.helmet
