from __future__ import annotations

from typing import TYPE_CHECKING

import pytest

from actions import MeleeAction
from components import Attack, Stats
from constants import Color
from exceptions import Impossible, MissingComponent, NoTarget

if TYPE_CHECKING:
    from tcod.ecs import Entity

    from gameworld import GameWorld
    from messagelog import MessageLog


@pytest.mark.depends(on=["MessageLog", "GameWorld"])
class TestMelee:
    def test_melee_player_attack_damage(
        self,
        gameworld_fixed_seed: GameWorld,
        wolf_one_below: Entity,
        message_log: MessageLog,
    ) -> None:
        old_hp = wolf_one_below.components[Stats].hp
        MeleeAction(
            gameworld_fixed_seed.player,
            (0, -1),
            gameworld_fixed_seed.map,
            gameworld_fixed_seed.rng,
            message_log,
        ).perform()
        new_hp = wolf_one_below.components[Stats].hp
        assert new_hp < old_hp

    def test_melee_player_attack_message_color(
        self,
        wolf_one_below: Entity,
        gameworld_fixed_seed: GameWorld,
        message_log: MessageLog,
    ) -> None:
        MeleeAction(
            gameworld_fixed_seed.player,
            (0, -1),
            gameworld_fixed_seed.map,
            gameworld_fixed_seed.rng,
            message_log,
        ).perform()
        assert message_log.messages[-1].color == Color.PLAYER_ATTACK

    def test_melee_player_attack_message_text(
        self,
        wolf_one_below: Entity,
        gameworld_fixed_seed: GameWorld,
        message_log: MessageLog,
    ) -> None:
        MeleeAction(
            gameworld_fixed_seed.player,
            (0, -1),
            gameworld_fixed_seed.map,
            gameworld_fixed_seed.rng,
            message_log,
        ).perform()
        assert message_log.messages[-1].plain_text.find("You attack the wolf") != -1

    def test_melee_player_attack_empty_tile(
        self, gameworld_fixed_seed: GameWorld, message_log: MessageLog
    ) -> None:
        with pytest.raises(Impossible) as exc:
            MeleeAction(
                gameworld_fixed_seed.player,
                (1, 0),
                gameworld_fixed_seed.map,
                gameworld_fixed_seed.rng,
                message_log,
            ).perform()
        assert exc.type is NoTarget

    def test_melee_enemy_attack(
        self,
        gameworld_fixed_seed: GameWorld,
        wolf_one_below: Entity,
        message_log: MessageLog,
    ) -> None:
        old_hp = gameworld_fixed_seed.player.components[Stats].hp
        MeleeAction(
            wolf_one_below,
            (0, 1),
            gameworld_fixed_seed.map,
            gameworld_fixed_seed.rng,
            message_log,
        ).perform()
        new_hp = gameworld_fixed_seed.player.components[Stats].hp
        assert new_hp < old_hp

    def test_melee_enemy_attack_message_color(
        self,
        wolf_one_below: Entity,
        gameworld_fixed_seed: GameWorld,
        message_log: MessageLog,
    ) -> None:
        MeleeAction(
            wolf_one_below,
            (0, 1),
            gameworld_fixed_seed.map,
            gameworld_fixed_seed.rng,
            message_log,
        ).perform()
        assert message_log.messages[-1].color == Color.ENEMY_ATTACK

    def test_melee_player_miss(
        self,
        gameworld_fixed_seed: GameWorld,
        strong_entity: Entity,
        message_log: MessageLog,
    ) -> None:
        old_hp = strong_entity.components[Stats].hp
        MeleeAction(
            gameworld_fixed_seed.player,
            (0, -1),
            gameworld_fixed_seed.map,
            gameworld_fixed_seed.rng,
            message_log,
        ).perform()
        new_hp = strong_entity.components[Stats].hp
        assert old_hp == new_hp

    @pytest.mark.xfail
    def test_melee_player_kill(
        self,
        gameworld_fixed_seed: GameWorld,
        weak_entity: Entity,
        message_log: MessageLog,
    ) -> None:
        MeleeAction(
            gameworld_fixed_seed.player,
            (0, -1),
            gameworld_fixed_seed.map,
            gameworld_fixed_seed.rng,
            message_log,
        ).perform()
        assert (
            len(gameworld_fixed_seed.registry[weak_entity.uid].components.keys()) == 0
        )

    def test_melee_player_kill_xp(
        self,
        gameworld_fixed_seed: GameWorld,
        weak_entity: Entity,
        message_log: MessageLog,
    ) -> None:
        old_xp = gameworld_fixed_seed.player.components[Stats].xp
        MeleeAction(
            gameworld_fixed_seed.player,
            (0, -1),
            gameworld_fixed_seed.map,
            gameworld_fixed_seed.rng,
            message_log,
        ).perform()
        new_xp = gameworld_fixed_seed.player.components[Stats].xp
        assert new_xp == old_xp + 10

    def test_melee_player_kill_message(
        self,
        gameworld_fixed_seed: GameWorld,
        weak_entity: Entity,
        message_log: MessageLog,
    ) -> None:
        MeleeAction(
            gameworld_fixed_seed.player,
            (0, -1),
            gameworld_fixed_seed.map,
            gameworld_fixed_seed.rng,
            message_log,
        ).perform()
        assert message_log.messages[-1].plain_text.find("killing it") != -1

    def test_melee_missing_stats(
        self, gameworld_fixed_seed: GameWorld, wolf_one_below: Entity, message_log: MessageLog
    ) -> None:
        del wolf_one_below.components[Stats]
        action = None
        with pytest.raises(MissingComponent) as exc:
            action = MeleeAction(
                gameworld_fixed_seed.player,
                (0, -1),
                gameworld_fixed_seed.map,
                gameworld_fixed_seed.rng,
                message_log,
            )
        assert exc.type is MissingComponent
        if action is not None:
            action.perform()

    def test_melee_missing_attack(
        self, gameworld_fixed_seed: GameWorld, wolf_one_below: Entity, message_log: MessageLog
    ) -> None:
        del wolf_one_below.components[Attack]
        with pytest.raises(Impossible) as exc:
            MeleeAction(
                wolf_one_below,
                (0, 1),
                gameworld_fixed_seed.map,
                gameworld_fixed_seed.rng,
                message_log,
            ).perform()
        assert exc.type is MissingComponent

    def test_melee_player_death(
        self,
        gameworld_fixed_seed: GameWorld,
        strong_entity: Entity,
        message_log: MessageLog,
    ) -> None:
        MeleeAction(
            strong_entity,
            (0, 1),
            gameworld_fixed_seed.map,
            gameworld_fixed_seed.rng,
            message_log,
        ).perform()
        assert gameworld_fixed_seed.player.components[Stats].hp <= 0
