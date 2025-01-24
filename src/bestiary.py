"""Bestiary"""

from __future__ import annotations

from constants import tags


class _Kill:
    def __init__(self):
        self.normal = 0
        self.alpha = 0

    def __eq__(self, other: _Kill) -> bool:
        return self.normal == other.normal and self.alpha == other.alpha

    def add_murder(self) -> None:
        self.normal += 1

    def add_alpha_murder(self) -> None:
        self.alpha += 1

    @property
    def total(self) -> int:
        return self.alpha + self.normal


class Bestiary:
    """Handles recording the player's kills"""

    murders: dict[tags.Enemies, _Kill]

    def __init__(self):
        self.murders = {enemy_type: _Kill() for enemy_type in tags.Enemies}

    def __eq__(self, other: Bestiary) -> bool:
        return self.murders == other.murders

    def record_kill(self, *, enemy_type: tags.Enemies, alpha: bool = False) -> None:
        if alpha:
            self.murders[enemy_type].add_alpha_murder()
        else:
            self.murders[enemy_type].add_murder()

    def total_murders(self, enemy_type: tags.Enemies | None = None) -> int:
        if enemy_type is None:
            kills = 0
            for etype in list(tags.Enemies):
                kills += self.murders[etype].total
            return kills
        return self.murders[enemy_type].total
