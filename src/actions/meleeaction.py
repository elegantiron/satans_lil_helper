from __future__ import annotations

from typing import TYPE_CHECKING

import arcade

from components import Name, Stats
from constants import colors
from exceptions import Impossible, MissingComponent
from utils import get_damage, get_damage_factor

from .actionwithdirection import ActionWithDirection

if TYPE_CHECKING:
    from random import Random

    import tcod.ecs

    from engine import Engine
    from gamemap import GameMap


class MeleeAction(ActionWithDirection):
    def __init__(
        self,
        entity: tcod.ecs.Entity,
        direction: tuple[int, int],
        gamemap: GameMap,
        rng: Random,
    ):
        super().__init__(entity, direction, gamemap)
        self.rng = rng

    def perform(self) -> None:
        target = self.target_entity
        actor = self.entity
        if not target:
            raise Impossible
        actor_name = actor.components.get(Name, "the mysterious stranger")
        if isinstance(actor_name, Name):
            actor_name = actor_name.name
        target_name = target.components.get(Name, "the mysterious stranger")
        if isinstance(target_name, Name):
            target_name = target_name.name.capitalize()

        description = f"{actor_name.capitalize()} attacks {target_name}"

        t_stats = target.components.get(Stats, None)
        a_stats = actor.components.get(Stats, None)
        if a_stats is None or t_stats is None:
            raise MissingComponent
        damage_factor = get_damage_factor(
            target_level=t_stats, actor_level=a_stats, rng=self.rng
        )
        dice = 1
        sides = 8
        damage = get_damage(
            damage_factor=damage_factor, dice=dice, sides=sides, rng=self.rng
        )
        t_stats.hp -= damage
        if damage == 0:
            description = f"{description} but deals no damage."
        else:
            description = f"{description}, dealing {damage} damage"
        if t_stats.hp <= 0:
            description = f"{description} and killing it."
            a_stats.xp += t_stats.xp_granted
            del target.registry[target]
        else:
            description = f"{description}."
        view: Engine = arcade.get_window().view
        view.message_log.add_message(description, colors.PlayerAttack)
