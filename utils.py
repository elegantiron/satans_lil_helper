from __future__ import annotations
from typing import TYPE_CHECKING
import hashlib
import hmac
import lzma

import dill as pickle

if TYPE_CHECKING:
    import tcod.ecs

HMAC_KEY = b"adsfauioerbasfhdjkagvyudis"


def load_data(path) -> bytes:
    signer = hmac.new(HMAC_KEY, digestmod=hashlib.blake2b)
    save_data = ""
    try:
        with open(path, "rb") as f:
            mac_data = f.read(signer.digest_size)
            save_data = f.read()
        signer.update(save_data)
        computed_mac = signer.digest()
        if hmac.compare_digest(mac_data, computed_mac):
            return pickle.loads(lzma.decompress(save_data))
        else:
            raise Exception
    except FileNotFoundError:
        return None


def save_data(data, path):
    raw_data = pickle.dumps(data)
    save_data = lzma.compress(raw_data)
    signer = hmac.new(HMAC_KEY, digestmod=hashlib.blake2b)
    signer.update(save_data)
    mac_result = signer.digest()
    with open(path, "wb") as f:
        f.write(mac_result)
    with open(path, "ab") as f:
        f.write(save_data)


def pixel_to_grid(coordinates: tuple[int, int]) -> tuple[int, int]:
    return coordinates[0] // 32, coordinates[1] // 32


def grid_to_pixel(coordinates: tuple[int, int]) -> tuple[int, int]:
    return coordinates[0] * 32, coordinates[1] * 32


def move_entity(entity: tcod.ecs.Entity, dest: tcod.ecs.Registry) -> tcod.ecs.Entity:
    target = dest[entity.uid]
    target.components = entity.components
    target.tags = entity.tags
    target.relation_tags_many = entity.relation_tags_many
    target.relation_components = entity.relation_components

    relation_targets = set()
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
    return bool(
        entity.components
        or entity.tags
        or entity.relation_tags_many
        or entity.relation_components
    )
