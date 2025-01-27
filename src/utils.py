"""Various helper functions"""

from __future__ import annotations

import hashlib
import hmac
import lzma
from typing import TYPE_CHECKING

import dill as pickle  # type: ignore

from components import Stats
from constants import EntityTags
from exceptions import HashError

if TYPE_CHECKING:
    import random
    from pathlib import Path

    import tcod.ecs

HMAC_KEY = b"adsfauioerbasfhdjkagvyudis"


def load_data(path: Path) -> bytes | None:
    """Load saved data"""
    signer = hmac.new(HMAC_KEY, digestmod=hashlib.blake2b)  # type: ignore
    data_to_load: bytes
    try:
        with path.open("rb") as f:
            mac_data = f.read(signer.digest_size)
            data_to_load = f.read()
        signer.update(data_to_load)
        computed_mac = signer.digest()
        if hmac.compare_digest(mac_data, computed_mac):
            return pickle.loads(lzma.decompress(data_to_load))
        raise HashError
    except FileNotFoundError:
        return None


def save_data(data: bytes, path: Path) -> None:
    """Save the game's data"""
    raw_data = pickle.dumps(data)
    data_to_save = lzma.compress(raw_data)
    signer = hmac.new(HMAC_KEY, digestmod=hashlib.blake2b)  # type: ignore
    signer.update(data_to_save)
    mac_result = signer.digest()
    with path.open("wb") as f:
        f.write(mac_result)
    with path.open("ab") as f:
        f.write(data_to_save)


def pixel_to_grid(coordinates: tuple[int, int]) -> tuple[int, int]:
    """Convert pixel coordinates to grid coordinates"""
    return coordinates[0] // 32, coordinates[1] // 32


def grid_to_pixel(coordinates: tuple[int, int]) -> tuple[int, int]:
    """Convert grid coordinates to pixel coordinates"""
    return coordinates[0] * 32, coordinates[1] * 32


def move_entity(entity: tcod.ecs.Entity, dest: tcod.ecs.Registry) -> tcod.ecs.Entity:
    """Move an arbitrary entity from one map to another"""
    target = dest[entity.uid]
    target.components = entity.components
    target.tags = entity.tags
    target.relation_tags_many = entity.relation_tags_many  # type: ignore
    target.relation_components = entity.relation_components  # type: ignore

    relation_targets: set[tcod.ecs.Entity] = set()
    for r_tag, r_targets in entity.relation_tags_many.items():
        relation_targets |= r_targets
        target.relation_tags_many[r_tag] = (
            dest[r_target.uid] for r_target in r_targets
        )

    for r_key, r_target_components in entity.relation_components.items():
        relation_targets |= r_target_components.keys()
        target.relation_components[r_key] = {
            dest[r_target.uid]: r_component
            for r_target, r_component in r_target_components.items()
        }

    entity.clear()
    for r_target in relation_targets:
        if has_any_components(r_target):
            move_entity(r_target, dest)
    return target


def has_any_components(entity: tcod.ecs.Entity) -> bool:
    """Check if an entity has any components"""
    return bool(
        entity.components
        or entity.tags
        or entity.relation_tags_many
        or entity.relation_components
    )


def get_total_stats(entity: tcod.ecs.Entity) -> Stats:
    """Calculate an entity's stats, including equipment"""
    entity_stats = entity.components[Stats]
    total_stats = Stats(
        entity_stats.hp,
        entity_stats.mp,
        entity_stats.strength,
        entity_stats.magic,
        entity_stats.pdef,
        entity_stats.mdef,
        entity_stats.evasion,
        entity_stats.crit,
        entity_stats.sight,
        entity_stats.light,
    )
    for relation in entity.relation_tags_many[EntityTags.EQUIPPED]:
        r_stats = relation.components.get(Stats, None)
        if r_stats is not None:
            total_stats += r_stats
    return total_stats


def get_neighbors(x: int, y: int, tiles: list[list[int]]) -> int:
    """Get a tile's neighbor count"""
    dirs = [
        (dx, dy) for dx in range(-1, 2) for dy in range(-1, 2) if (dx, dy) != (0, 0)
    ]
    neighbors = tiles[x][y]
    for dx, dy in dirs:
        cx = x + dx
        cy = y + dy
        if cx in range(len(tiles)) and cy in range(len(tiles[cx])):
            neighbors += tiles[cx][cy]
        else:
            neighbors += 1
    return neighbors


def get_damage_factor(*, t_stats: Stats, a_stats: Stats, rng: random.Random) -> float:
    """Returns an attack's damage factor"""
    t_level = t_stats.level
    a_level = a_stats.level
    a_crit = a_stats.crit
    roll = rng.randint(1, 100)
    if roll <= max(0, t_level - a_level) or roll == 1:
        return 0
    if roll >= 95 - a_crit:
        return 2
    if roll <= 10:
        return 0.5
    if roll <= 60:
        return 1
    return 1.25


def get_damage(
    *, damage_factor: float, dice: int, sides: int, rng: random.Random, strength: int
) -> int:
    """Get an attack's damage"""
    dmg = strength
    for _ in range(dice):
        dmg += rng.randint(1, sides)
    return int(dmg * damage_factor)
