from __future__ import annotations

from dataclasses import dataclass

from .tags import Abilities

# None requirement means no prerequisites
# None is used for any place holders


@dataclass(frozen=True, eq=False, kw_only=True)
class Skill:
    skill_id: Abilities
    name: str
    description: str
    prereqs: Abilities | None = None


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
        prereqs=(
            Abilities.ELEMENTAL_AFFINITY_I | Abilities.ELEMENTAL_AFFINITY_II
        ),
    ),
]
