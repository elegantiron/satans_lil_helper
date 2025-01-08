from __future__ import annotations

from typing import TYPE_CHECKING

from components import Equippable, Skills
from constants import EntityTags
from exceptions import Impossible, MissingComponent

from .baseaction import Action

if TYPE_CHECKING:
    import tcod.ecs


class ItemAction(Action):
    def __init__(self, entity: tcod.ecs.Entity, item: tcod.ecs.Entity):
        super().__init__(entity)
        self.item = item


class EquipAction(ItemAction):
    def perform(self) -> None:
        equippable = self.item.components.get(Equippable, None)
        if equippable is None:
            raise MissingComponent("An entity tried to equip an unequippable item.")
        skills = self.entity.components.get(Skills, None)
        if skills is None:
            raise MissingComponent("An entity without skills tried to equip an item")
        if equippable.requirements not in skills.onetime:
            raise Impossible
        if (
            self.item not in self.entity.relation_tags_many[EntityTags.HOLDING]
            or self.entity != self.item.relation_tag[EntityTags.HELD_BY]
        ):
            raise Impossible("You cannot equip an item you are not holding.")
        self.item.tags.add(EntityTags.EQUIPPED)
        self.item.relation_tag[EntityTags.EQUIPPED_BY] = self.entity


class UnequipAction(ItemAction):
    def perform(self):
        self.item.relation_tag[EntityTags.EQUIPPED_BY] = None
        self.item.tags.discard(EntityTags.EQUIPPED)
