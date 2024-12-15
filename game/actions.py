from __future__ import annotations
from typing import TYPE_CHECKING

import numpy as np

from game.components import Health, Inventory, Level, Name, Position
from game.constants import Tags, Tile
from game.exceptions import Impossible, InventoryFull, MissingComponent, PathBlocked
from game.utils import get_damage, get_damage_factor

if TYPE_CHECKING:
    import tcod.ecs
    from random import Random
    import numpy.typing as npt


class Action:
    def __init__(self, entity: tcod.ecs.Entity, rng: Random):
        self.rng = rng
        self.entity = entity

    def perform(self) -> None:
        raise NotImplementedError


class PickupAction(Action):
    def perform(self) -> None:
        inventory = self.entity.components.get(Inventory, None)
        if inventory is None:
            raise MissingComponent
        if (
            len(
                set(
                    self.entity.registry.Q.all_of(
                        tags=[Tags.Item], components=[Position]
                    ).none_of(relations=[..., Tags.Holding, None])
                )
            )
            >= inventory.size
        ):
            raise InventoryFull
        location = self.entity.components.get(Position, None)
        if location is None:
            raise MissingComponent
        for ent in (
            entity
            for entity in self.entity.registry.Q.all_of(
                components=[Position], tags=[Tags.Item]
            )
            if (
                entity.components[Position].x == location.x
                and entity.components[Position].y == location.y
            )
        ):
            if (
                len(
                    set(
                        self.entity.registry.Q.all_of(
                            relations=[self.entity, Tags.Holding, None]
                        )
                    )
                )
                >= inventory.size
            ):
                raise InventoryFull
            ent.relation_tag[Tags.HeldBy] = self.entity
            self.entity.relation_tags_many[Tags.Holding].add(ent)

            del ent[Position]


class ActionWithDirection(Action):
    def __init__(
        self,
        entity: tcod.ecs.Entity,
        direction: tuple[int, int],
        rng: Random,
        gamemap: npt.NDArray,
    ):
        super().__init__(entity=entity, rng=rng)
        self.gamemap = gamemap
        self.direction = direction
        position = entity.components[Position]
        self.target_xy = (self.dx + position.x, self.dy + position.y)
        self.target_entity = None
        for ent in (
            entity
            for entity in self.entity.registry.Q.all_of(components=[Position])
            if (
                entity.components[Position].x == self.target_x
                and entity.components[Position].y == self.target_y
            )
        ):
            self.target_entity = ent

    @property
    def dx(self) -> int:
        return self.direction[0]

    @property
    def dy(self) -> int:
        return self.direction[1]

    @property
    def target_x(self) -> int:
        return self.target_xy[0]

    @property
    def target_y(self) -> int:
        return self.target_xy[1]


class MeleeAction(ActionWithDirection):
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


class MovementAction(ActionWithDirection):
    def perform(self) -> None:
        if (self.target_x, self.target_y) not in np.ndindex(self.gamemap.shape):
            raise PathBlocked
        if not self.gamemap[Tile.Walkable]:
            raise PathBlocked
        if self.target_entity is not None:
            raise PathBlocked

        position = self.entity.components[Position]
        position.x += self.dx
        position.y += self.dy


class BumpAction(ActionWithDirection):
    def perform(self) -> None:
        if self.target_entity:
            if Tags.Hostile in self.target_entity.tags:
                return MeleeAction(self.entity, self.direction).perform()
            elif Tags.Friendly in self.target_entity.tags:
                return TalkAction(self.entity, self.direction).perform()


class TalkAction(ActionWithDirection):
    pass
