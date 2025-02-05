"""Definitions for the Melee Action"""

from __future__ import annotations

from typing import TYPE_CHECKING

from components import Attack, Name, Stats
from constants import Color
from exceptions import MissingComponent, NoTarget
from utils import get_damage, get_damage_factor

from .actionwithdirection import ActionWithDirection

if TYPE_CHECKING:
    from random import Random

    import tcod.ecs

    from gamemap import GameMap
    from messagelog import MessageLog


class MeleeAction(ActionWithDirection):
    """For performing melee attacks"""

    def __init__(
        self,
        entity: tcod.ecs.Entity,
        direction: tuple[int, int],
        gamemap: GameMap,
        rng: Random,
        message_log: MessageLog,
    ) -> None:
        super().__init__(entity, direction, gamemap, message_log)
        self.kill = False
        self.rng = rng
        self._pre_state = self.rng.getstate()
        target = self.target_entity
        actor = self.entity
        if not target:
            raise NoTarget
        actor_name = actor.components.get(Name, Name("the mysterious stranger")).name
        t_name = target.components.get(Name, Name("the mysterious stranger"))
        target_name = ""
        if t_name.definite_article is not None:
            target_name = f"{target_name}{t_name.definite_article} "
        target_name = f"{target_name}{t_name.name}"

        if actor_name == "you":
            description = f"{actor_name.capitalize()} attack {target_name}"
        else:
            description = f"{actor_name.capitalize()} attacks {target_name}"

        t_stats = target.components.get(Stats, None)
        a_stats = actor.components.get(Stats, None)
        if a_stats is None or t_stats is None:
            raise MissingComponent
        damage_factor = get_damage_factor(
            t_stats=t_stats, a_stats=a_stats, rng=self.rng
        )
        attack = actor.components.get(Attack, None)
        if attack is None:
            raise MissingComponent("An entity with out an attack tried to attack.")
        self.damage = get_damage(
            damage_factor=damage_factor,
            dice=attack.dice,
            sides=attack.sides,
            rng=self.rng,
            strength=int(a_stats.strength),
        )
        if self.damage == 0:
            description = f"{description} but deals no damage."
        else:
            description = f"{description}, dealing {self.damage} damage"
        if t_stats.hp <= self.damage:
            self.kill = True
            description = f"{description} and killing it."

        else:
            description = f"{description}."
        self.description = description
        self._post_state = self.rng.getstate()

    def perform(self) -> None:
        if self.target_entity is None:
            raise NoTarget
        self.target_entity.components[Stats].hp -= self.damage
        if self.kill:
            self.entity.components[Stats].xp += self.target_entity.components[
                Stats
            ].xp_granted
        self.rng.setstate(self._post_state)

        if self.entity is self.gamemap.player:
            self.message_log.add_message(self.description, Color.PLAYER_ATTACK)
        else:
            self.message_log.add_message(self.description, Color.ENEMY_ATTACK)

    def rollback(self) -> None:
        if self.target_entity is None:
            raise NoTarget
        self.target_entity.components[Stats].hp += self.damage
        if self.kill:
            self.entity.components[Stats].xp -= self.target_entity.components[
                Stats
            ].xp_granted
        del self.message_log.messages[-1]
        self.rng.setstate(self._pre_state)
