"""Ability enums"""

from enum import Enum, Flag, auto


class Enemies(Flag):
    """Enemy abilities"""

    PACK_TACTICS = auto()
    DARK_VISION = auto()
    GNAW = auto()
    HOWL = auto()


class General(Flag):
    ELEMENTAL_AFFINITY_I = auto()
    ELEMENTAL_AFFINITY_II = auto()
    ELEMENTAL_AFFINITY_III = auto()

    LIGHT_UP = auto()

    STATUS_MASTER_I = auto()
    STATUS_MASTER_II = auto()
    STATUS_MASTER_III = auto()

    ITEM_SLOT_SHIELD = auto()
    ITEM_SLOT_ARMOR = auto()
    ITEM_SLOT_HELM = auto()
    ITEM_SLOT_GAUNTLETS = auto()
    ITEM_SLOT_GREAVES = auto()
    ITEM_SLOT_BOOTS = auto()


class Repeatable(Enum):
    STRENGTH_UP = auto()
    MAGIC_UP = auto()
    PDEF_UP = auto()
    MDEF_UP = auto()


class Warrior(Flag):
    SHIELD_UP = auto()
    CHARGE = auto()
    
    FLAMING_WEAPON = auto()
    IMPROVED_FLAMING_WEAPON = auto()
    ELECTRIC_WEAPON = auto()
    IMPROVED_ELECTRIC_WEAPON = auto()
    COLD_WEAPON = auto()
    IMPROVED_COLD_WEAPON = auto()
    SERRATED_WEAPON = auto()
    
    SHIELD_WALL = auto()
    SHIELD_WALL_TOSS = auto()
    
    IMPROVED_CHARGE = auto()
    SHOVING_CHARGE = auto()
    WALL_SLAM = auto()
    
    FIRE_RESISTANT = auto()
    FIRE_BODY = auto()
    GREATER_FIRE_BODY = auto()
    
    ELECTRIC_RESISTANT = auto()
    SHOCK_COLLAR = auto()
    IMPROVED_SHOCK_COLLAR = auto()
    
    COLD_RESISTANT = auto()
    FROST_STEP = auto()
    IMPROVED_FROST_STEP = auto()
    
    SHIELD_SLAM = auto()
    
    TARGET_HEAD = auto()
    TARGET_LEG = auto()
    TARGET_ARM = auto()
    DISARM = auto()
    
    LEAPING_CHARGE = auto()
    
    ENCHANTED_SHIELD = auto()
    RETRIBUTIVE_STRIKE = auto()
    
    SHIELD_SPELL_ABSORPTION = auto()
    SHIELD_SPELL_RETRIBUTION = auto()
    
    FIELD_MEDICINE = auto()
    SPLINT = auto()
    FIELD_HOSPITAL = auto()
    
    SWORD_TRAINING = auto()
    AXE_TRAINING = auto()
    HAMMER_TRAINING = auto()
    MACE_TRAINING = auto()
    SPEAR_TRAINING = auto()
    PICK_TRAINING = auto()
    WHIP_TRAINING = auto()
    SCYTHE_TRAINING = auto()
    POLEARM_TRAINING = auto()
    RAPIER_TRAINING = auto()
