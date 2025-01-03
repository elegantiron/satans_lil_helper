from __future__ import annotations

import attrs
from constants import ItemType


@attrs.define(kw_only=True)
class Equipment:
    one_hand: bool = False
    two_hand: bool = False
    mundane: bool = False
    magical: bool = False
    sec_weap: bool = False
    shield: bool = False
    body: bool = False
    hands: bool = False
    feet: bool = False
    legs: bool = False
    head: bool = False

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
                return self.sec_weap
            case ItemType.Shield:
                return self.shield
            case ItemType.BodyArmor:
                return self.body
            case ItemType.Gauntlets:
                return self.hands
            case ItemType.Boots:
                return self.feet
            case ItemType.Greaves:
                return self.legs
            case ItemType.Helmet:
                return self.head
