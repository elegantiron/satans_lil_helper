from __future__ import annotations

import random
from typing import TYPE_CHECKING

import pytest
from tcod.ecs import Entity, Registry

from abilities import Abilities, effects
from components import Skills, Stats
from entities import professions
from exceptions import Impossible, MissingComponent

if TYPE_CHECKING:
    from collections.abc import Iterator


@pytest.fixture(scope="class")
def registry() -> Registry:
    return Registry()


@pytest.fixture
def entity(registry: Registry) -> Iterator[Entity]:
    new_entity = registry.new_entity()
    professions.warrior_class(new_entity, (5, 5), random.Random())
    yield new_entity
    new_entity.clear()


@pytest.fixture
def statup_effect() -> effects.StatUp:
    return effects.StatUp(Stats(strength=1))


@pytest.fixture
def addflag_effect() -> effects.AddFlag:
    return effects.AddFlag(Abilities.COLD_WEAPON)


class TestSatUp:
    def test_statup_application(
        self, entity: Entity, statup_effect: effects.StatUp
    ) -> None:
        old_strength = entity.components[Stats].strength
        statup_effect.apply_skill_effect(entity)
        new_strength = entity.components[Stats].strength
        assert new_strength == old_strength + 1

    def test_statup_rollback(
        self, entity: Entity, statup_effect: effects.StatUp
    ) -> None:
        old_strength = entity.components[Stats].strength
        statup_effect.roll_back_skill_effect(entity)
        new_strength = entity.components[Stats].strength
        assert new_strength == old_strength - 1

    def test_statup_application_no_stats(
        self, entity: Entity, statup_effect: effects.StatUp
    ) -> None:
        del entity.components[Stats]
        with pytest.raises(Impossible) as excinfo:
            statup_effect.apply_skill_effect(entity)
        assert excinfo.type is MissingComponent

    def test_statup_rollback_no_stats(
        self, entity: Entity, statup_effect: effects.StatUp
    ) -> None:
        del entity.components[Stats]
        with pytest.raises(Impossible) as excinfo:
            statup_effect.roll_back_skill_effect(entity)
        assert excinfo.type is MissingComponent


class TestAddFlag:
    def test_addflag_application(
        self, entity: Entity, addflag_effect: effects.AddFlag
    ) -> None:
        addflag_effect.apply_skill_effect(entity)
        assert Abilities.COLD_WEAPON in entity.components[Skills].onetime

    def test_addflag_rollback(
        self, entity: Entity, addflag_effect: effects.AddFlag
    ) -> None:
        entity.components[Skills].onetime |= Abilities.COLD_WEAPON
        addflag_effect.roll_back_skill_effect(entity)
        assert Abilities.COLD_WEAPON not in entity.components[Skills].onetime
