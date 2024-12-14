"""Defines actions that an acting entity might take."""

from __future__ import annotations

from typing import TYPE_CHECKING

from components import (
    Health,
    Inventory,
    Level,
    Name,
    Position,
)
from constants import TAGS
from exceptions import Impossible, InventoryFull, MissingComponent, PathBlocked
from tools import get_damage, get_damage_factor

if TYPE_CHECKING:
    import tcod.ecs


class Action:
    """Base action that other actions inherit from."""
    def __init__(self, entity: tcod.ecs.Entity):
        self.entity = entity

    def perform(self) -> None:
        raise NotImplementedError


class PickupAction(Action):
    """Picks up items.
    
    Picks up any item on the same tile as the given entity. Also checks to
    be sure that the entity has enough space in its inventory."""
    def perform(self) -> None:
        inventory = self.entity.components.get(Inventory, None)
        if inventory is None:
            raise MissingComponent(
                "An entity without an inventory is trying to pick up an item."
            )
        if (
            len(
                set(
                    self.entity.registry.Q.all_of(
                        tags=[TAGS.ITEM], components=[Position]
                    ).none_of(relations=[..., TAGS.HOLDING, None])
                )
            )
            >= inventory.size
        ):
            raise InventoryFull
        location = self.entity.components.get(Position, None)
        if location is None:
            raise MissingComponent(
                "An entity without a position component is trying to pick up an item."
            )
        for ent in (
            entity
            for entity in self.entity.registry.Q.all_of(
                components=[Position], tags=[TAGS.ITEM]
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
                            relations=[self.entity, TAGS.HOLDING, None]
                        )
                    )
                )
                >= inventory.size
            ):
                raise InventoryFull
            ent.relation_tag[TAGS.HELD_BY] = self.entity
            self.entity.relation_tags_many[TAGS.HOLDING].add(ent)

            del ent[Position]


class ActionWithDirection(Action):
    """Class to implement directional acitons.
    
    This class only implements some common functions for every action
    that involves one of the adjacent tiles."""
    def __init__(self, entity: int, direction: tuple[int, int]):
        super().__init__(entity)
        self.direction = direction
        position = entity[Position]
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
    """Strike an adjacent entity."""
    def perform(self) -> None:
        """Perform a melee attack action.
        
        .. todo::
           - [ ] Log the attack description to the message log"""
        target = self.target_entity
        actor_name = self.entity.components[Name].name
        target_name = self.target_entity.components[Name].name
        description = f"{actor_name.capitalize()} attacks {target_name.capitalize()}"
        if not target:
            raise Impossible("There is nothing to attack.")
        target_level = self.target_entity.components[Level]
        actor_level = self.entity.components[Level]
        damage_factor = get_damage_factor(
            target_level=target_level.level, actor_level=actor_level
        )
        damage = get_damage(damage_factor=damage_factor)

        target_health = self.target_entity.components[Health]
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


class MeleeItemAction(ActionWithDirection):
    """Uses an item as part of a melee range action.
    
    .. todo::
       - [ ] Implement melee item action"""
    pass


class MovementAction(ActionWithDirection):
    """Moves an entity."""
    def perform(self) -> None:
        """Performs the movement.
        
        .. todo::
           - [ ] Check if the destination is in bounds
           - [ ] Check if the destination is walkable
           - [ ] Check if the destination has a blocking entity
           """
        pass
        in_bounds = True
        if not in_bounds:
            raise PathBlocked
        walkable = True
        if not walkable:
            raise PathBlocked
        blocking_entity = False
        if blocking_entity:
            raise PathBlocked

        position = self.entity.components[Position]
        position.x += self.dx
        position.y += self.dy


class TalkAction(ActionWithDirection):
    """Talk to a friendly NPC
    
    .. todo::
       - [ ] Implement talking action"""
    pass


class BumpAction(ActionWithDirection):
    """Perform an action based on what's in the target tile.
    
    Useful to allow the player to attempt to walk into an enemy or
    a friendly NPC and interact with them instead of just failing
    to move into the tile."""
    def perform(self) -> None:
        if self.target_entity:
            if TAGS.HOSTILE in self.target_entity.tags:
                return MeleeAction(self.entity, self.direction)
            elif TAGS.FRIENDLY in self.target_entity.tags:
                return TalkAction(self.entity, self.direction)
        return MovementAction(self.entity, self.direction)
