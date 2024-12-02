from __future__ import annotations

from typing import TYPE_CHECKING

from components.base_component import BaseComponent

if TYPE_CHECKING:
    from entity import Actor


class BaseStatusEffect(BaseComponent):
    parent: Actor

    def __init__(self, duration: int):
        self.duration = duration

    def proc(self) -> None:
        """Reduces the number of turns remaining"""
        self.duration -= 1
        if self.duration <= 0:
            self.remove()

    def remove(self) -> None:
        """Once it's run its course, the status effect should go away"""
        entity = self.parent
        entity.status_effects.remove(self)


class Poison(BaseStatusEffect):
    def __init__(self, duration: int, damage: int):
        self.damage = damage
        super().__init__(duration)

    def proc(self) -> None:
        self.parent.fighter.take_damage(self.damage)
        super().proc()


class Regen(BaseStatusEffect):
    def __init__(self, duration: int, healing: int):
        self.healing = healing
        super().__init__(duration)

    def proc(self) -> None:
        self.parent.fighter.hp += self.healing
        super().proc()
