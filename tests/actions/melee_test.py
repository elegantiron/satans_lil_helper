from __future__ import annotations

from typing import TYPE_CHECKING, Final

import pytest
from tcod.ecs.entity import Entity

from actions import MeleeAction
from components import Attack, Position, Stats
from constants import Color
from entities import enemies
from exceptions import Impossible, MissingComponent
from gameworld import GameWorld
from messagelog import MessageLog

if TYPE_CHECKING:
    from tcod.ecs import Entity

# pylint: disable=redefined-outer-name

SEED: Final[float] = 1737855529.0953882


@pytest.fixture(scope="class")
def gameworld() -> GameWorld:
    return GameWorld(SEED)


@pytest.fixture
def entity(gameworld: GameWorld) -> Entity: # type: ignore
    nentity= gameworld.spawn_entity(
        enemies.forest.wolf,
        (
            gameworld.player.components[Position].x,
            gameworld.player.components[Position].y - 1,
        ),
    )
    yield nentity # type: ignore
    nentity.clear()


@pytest.fixture
def strong_entity(gameworld: GameWorld) -> Entity: # type: ignore
    sentity = gameworld.spawn_entity(
        enemies.testing.strong,
        (
            gameworld.player.components[Position].x,
            gameworld.player.components[Position].y - 1,
        ),
    )
    yield sentity # type: ignore
    sentity.clear()


@pytest.fixture
def weak_entity(gameworld: GameWorld) -> Entity: # type: ignore
    wentity = gameworld.spawn_entity(
        enemies.testing.weak,
        (
            gameworld.player.components[Position].x,
            gameworld.player.components[Position].y - 1,
        ),
    )
    yield wentity # type: ignore
    wentity.clear()


@pytest.fixture(scope="class")
def message_log() -> MessageLog:
    return MessageLog()


@pytest.mark.depends(on=["tests/messagelog_test.py", "tests/gameworld_test.py"])
class TestMelee:
    def test_melee_player_attack_damage(
        self, gameworld: GameWorld, entity: Entity, message_log: MessageLog
    ) -> None:
        old_hp = entity.components[Stats].hp
        MeleeAction(
            gameworld.player, (0, -1), gameworld.map, gameworld.rng, message_log
        ).perform()
        new_hp = entity.components[Stats].hp
        assert new_hp < old_hp

    def test_melee_player_attack_message_color(
        self, entity: Entity, gameworld: GameWorld, message_log: MessageLog
    ) -> None:
        MeleeAction(
            gameworld.player, (0, -1), gameworld.map, gameworld.rng, message_log
        ).perform()
        assert message_log.messages[-1].color == Color.PLAYER_ATTACK

    def test_melee_player_attack_message_text(
        self, entity: Entity, gameworld: GameWorld, message_log: MessageLog
    ) -> None:
        MeleeAction(
            gameworld.player, (0, -1), gameworld.map, gameworld.rng, message_log
        ).perform()
        assert message_log.messages[-1].plain_text.find("You attack the wolf") != -1

    def test_melee_player_attack_empty_tile(
        self, gameworld: GameWorld, message_log: MessageLog
    ) -> None:
        with pytest.raises(Impossible) as exc:
            MeleeAction(
                gameworld.player, (1, 0), gameworld.map, gameworld.rng, message_log
            ).perform()
        assert exc.type is Impossible

    def test_melee_enemy_attack(
        self, gameworld: GameWorld, entity: Entity, message_log: MessageLog
    ) -> None:
        old_hp = gameworld.player.components[Stats].hp
        MeleeAction(entity, (0, 1), gameworld.map, gameworld.rng, message_log).perform()
        new_hp = gameworld.player.components[Stats].hp
        assert new_hp < old_hp

    def test_melee_enemy_attack_message_color(
        self, entity: Entity, gameworld: GameWorld, message_log: MessageLog
    ) -> None:
        MeleeAction(entity, (0, 1), gameworld.map, gameworld.rng, message_log).perform()
        assert message_log.messages[-1].color == Color.ENEMY_ATTACK

    def test_melee_player_miss(
        self, gameworld: GameWorld, strong_entity: Entity, message_log: MessageLog
    ) -> None:
        old_hp = strong_entity.components[Stats].hp
        MeleeAction(
            gameworld.player, (0, -1), gameworld.map, gameworld.rng, message_log
        ).perform()
        new_hp = strong_entity.components[Stats].hp
        assert old_hp == new_hp

    def test_melee_player_kill(
        self, gameworld: GameWorld, weak_entity: Entity, message_log: MessageLog
    ) -> None:
        MeleeAction(
            gameworld.player, (0, -1), gameworld.map, gameworld.rng, message_log
        ).perform()
        assert len(gameworld.registry[weak_entity.uid].components.keys()) == 0

    def test_melee_player_kill_xp(
        self, gameworld: GameWorld, weak_entity: Entity, message_log: MessageLog
    ) -> None:
        old_xp = gameworld.player.components[Stats].xp
        MeleeAction(
            gameworld.player, (0, -1), gameworld.map, gameworld.rng, message_log
        ).perform()
        new_xp = gameworld.player.components[Stats].xp
        assert new_xp == old_xp + 10

    def test_melee_player_kill_message(
        self, gameworld: GameWorld, weak_entity: Entity, message_log: MessageLog
    ) -> None:
        MeleeAction(
            gameworld.player, (0, -1), gameworld.map, gameworld.rng, message_log
        ).perform()
        assert message_log.messages[-1].plain_text.find("killing it") != -1

    def test_melee_missing_stats(
        self, gameworld: GameWorld, entity: Entity, message_log: MessageLog
    ) -> None:
        del entity.components[Stats]
        action = MeleeAction(
            gameworld.player, (0, -1), gameworld.map, gameworld.rng, message_log
        )
        with pytest.raises(MissingComponent) as exc:
            action.perform()
        assert exc.type is MissingComponent

    def test_melee_missing_attack(
        self, gameworld: GameWorld, entity: Entity, message_log: MessageLog
    ) -> None:
        del entity.components[Attack]
        with pytest.raises(Impossible) as exc:
            MeleeAction(
                entity, (0, 1), gameworld.map, gameworld.rng, message_log
            ).perform()
        assert exc.type is MissingComponent

    def test_melee_player_death(
        self, gameworld: GameWorld, strong_entity: Entity, message_log: MessageLog
    ) -> None:
        MeleeAction(
            strong_entity, (0, 1), gameworld.map, gameworld.rng, message_log
        ).perform()
        assert gameworld.player.components[Stats].hp <= 0
