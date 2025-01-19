from __future__ import annotations

import abc
from typing import TYPE_CHECKING

from components import Skills, Stats
from exceptions import MissingComponent

if TYPE_CHECKING:
    import tcod.ecs

    from constants import abilities


class BaseSkillEffect(metaclass=abc.ABCMeta):
    @abc.abstractmethod
    def apply_skill_effect(self, target: tcod.ecs.Entity) -> None:
        raise NotImplementedError

    @abc.abstractmethod
    def roll_back_skill_effect(self, target: tcod.ecs.Entity) -> None:
        raise NotImplementedError


class StatUp(BaseSkillEffect):
    def __init__(self, stat_block: Stats) -> None:
        self.stat_block = stat_block

    def apply_skill_effect(self, target):
        entity_stats = target.components.get(Stats)
        if entity_stats is None:
            raise MissingComponent
        entity_stats += self.stat_block

    def roll_back_skill_effect(self, target):
        entity_stats = target.components.get(Stats)
        if entity_stats is None:
            raise MissingComponent
        entity_stats -= self.stat_block


class AddFlag(BaseSkillEffect):
    def __init__(self, skill_tag: abilities.Abilities) -> None:
        self.skill_tag = skill_tag

    def apply_skill_effect(self, target):
        skills = target.components.get(Skills)
        if skills is None:
            raise MissingComponent
        skills.onetime |= self.skill_tag

    def roll_back_skill_effect(self, target):
        skills = target.components.get(Skills)
        if skills is None:
            raise MissingComponent
        skills.onetime &= ~self.skill_tag
