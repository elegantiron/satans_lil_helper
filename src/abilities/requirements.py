from __future__ import annotations

from dataclasses import dataclass

from components import Stats

from . import effects
from .tags import Abilities


@dataclass(frozen=True, eq=False, kw_only=True)
class Skill:
    skill_id: Abilities
    name: str
    description: str
    prereqs: Abilities | None = None
    onetime: bool = True
    effects: list[effects.BaseSkillEffect] | None = None


SkillList: list[Skill] = [
    Skill(
        skill_id=Abilities.ELEMENTAL_AFFINITY_I,
        name="Elemental Affinity I",
        description="Increases all elemental damage by 1.",
    ),
    Skill(
        skill_id=Abilities.ELEMENTAL_AFFINITY_II,
        name="Elemental Affinity II",
        description="Increases all elemental damage by an additional 2",
        prereqs=Abilities.ELEMENTAL_AFFINITY_I,
    ),
    Skill(
        skill_id=Abilities.ELEMENTAL_AFFINITY_III,
        name="Elemental Affinity III",
        description="Increases all elemental damage by an additional 3",
        prereqs=Abilities.ELEMENTAL_AFFINITY_I | Abilities.ELEMENTAL_AFFINITY_II,
    ),
    Skill(
        skill_id=Abilities.STATUS_MASTER_I,
        name="Status Master I",
        description="Increases chance to apply status effects by 10%",
    ),
    Skill(
        skill_id=Abilities.STATUS_MASTER_II,
        name="Status Master II",
        description="Increases chance to apply status effects by an additional 15%",
        prereqs=Abilities.STATUS_MASTER_I,
    ),
    Skill(
        skill_id=Abilities.STATUS_MASTER_III,
        name="Status Master III",
        description="Increases chance to apply status effects by an additional 25%",
        prereqs=Abilities.STATUS_MASTER_II,
    ),
    Skill(
        skill_id=Abilities.STRENGTH_UP,
        name="Strength Up",
        description="Increases Strength by 1.",
        onetime=False,
        effects=[effects.StatUp(Stats(strength=1))],
    ),
    Skill(
        skill_id=Abilities.MAGIC_UP,
        name="Magic Up",
        description="Increases Magic by 1.",
        onetime=False,
        effects=[effects.StatUp(Stats(magic=1))],
    ),
    Skill(
        skill_id=Abilities.PDEF_UP,
        name="Physical Defense Up",
        description="Increases Physical Defense by 1.",
        onetime=False,
        effects=[effects.StatUp(Stats(pdef=1))]
    ),
    Skill(
        skill_id=Abilities.MDEF_UP,
        name="Magic Defense Up",
        description="Increases Magic Defense by 1.",
        onetime=False,
        effects=[effects.StatUp(Stats(mdef=1))]
    ),
    Skill(
        skill_id=Abilities.LIGHT_UP,
        name="Light Radius Up",
        description="Increases light radius by 1.",
        effects=[effects.StatUp(Stats(light=1)), effects.AddFlag(Abilities.LIGHT_UP)]
    ),
    Skill(
        skill_id=Abilities.ITEM_SLOT_SHIELD,
        name="Shield Training",
        description="Allows the character to use shields.",
    ),
    Skill(
        skill_id=Abilities.ITEM_SLOT_ARMOR,
        name="Armor Training",
        description="Allows the character to use body armor.",
    ),
    Skill(
        skill_id=Abilities.ITEM_SLOT_HELM,
        name="Helm Training",
        description="Allows the character to use helms.",
    ),
    Skill(
        skill_id=Abilities.ITEM_SLOT_GAUNTLETS,
        name="Gauntlet Training",
        description="Allows the character to use gauntlets",
    ),
    Skill(
        skill_id=Abilities.ITEM_SLOT_GREAVES,
        name="Greaves Training",
        description="Allows the character to use greaves",
    ),
    Skill(
        skill_id=Abilities.ITEM_SLOT_WEAPON,
        name="Weapon Training",
        description="Allows the character to use weapons",
    ),
    Skill(
        skill_id=Abilities.ITEM_SLOT_BOOTS,
        name="Boot Training",
        description="Allows the character to use boots",
    ),
    Skill(
        skill_id=Abilities.ITEM_TYPE_MAGICAL,
        name="Magical Item Training",
        description="Allows the character to use magical items",
    ),
    Skill(
        skill_id=Abilities.ITEM_TYPE_MUNDANE,
        name="Mundane Item Training",
        description="Allows the character to use mundane items",
    ),
]
