from __future__ import annotations

from typing import TYPE_CHECKING

from ..components import Health, Level, Name
from ..exceptions import Impossible, MissingComponent
from ..utils import get_damage, get_damage_factor
from .actionwithdirection import ActionWithDirection

if TYPE_CHECKING:
    from random import Random


class MeleeAction(ActionWithDirection):
    def __init__(self, entity, direction, gamemap, rng: Random):
        super().__init__(entity=entity, direction=direction, gamemap=gamemap)
        self.rng = rng

    def perform(self) -> None:
        target = self.target_entity
        actor_name = self.entity.components.get(Name, "the mysterious stranger")
        if isinstance(actor_name, Name):
            actor_name = actor_name.name
        target_name = self.target_entity.components.get(Name, "the mysterious stranger")
        if isinstance(target_name, Name):
            target_name = target_name.name

        description = f"{actor_name.capitalize()} attacks {target_name.capitalize()}"
        if not target:
            raise Impossible
        target_level = self.target_entity.components.get(Level, None)
        actor_level = self.entity.components.get(Level, None)
        if actor_level is None or target_level is None:
            raise MissingComponent
        damage_factor = get_damage_factor(
            target_level=target_level, actor_level=actor_level, rng=self.rng
        )
        dice = 1
        sides = 8
        damage = get_damage(
            damage_factor=damage_factor, dice=dice, sides=sides, rng=self.rng
        )
        target_health = self.target_entity.components.get(Health, None)
        target_health.hp -= damage
        if damage == 0:
            description = f"{description} but deals no damage."
        else:
            description = f"{description}, dealing {damage} damage"
        if target_health.hp <= 0:
            description = f"{description} and killing it."
            actor_level.xp += target_level.xp_granted
            del self.target_entity.registry[self.target_entity]
        else:
            description = f"{description}."