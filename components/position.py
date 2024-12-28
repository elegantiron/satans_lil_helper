from __future__ import annotations

import arcade

from constants import TILE_SIZE


class Position:
    def __init__(self, *, x: int = 0, y: int = 0, sprite: str | None = None):
        if sprite is not None:
            self.sprite = arcade.Sprite(sprite)
        else:
            self.sprite = arcade.Sprite(
                center_x=self._x * TILE_SIZE, center_y=self._y * TILE_SIZE
            )
        self.x = x
        self.y = y

    @property
    def x(self) -> int:
        return self._x

    @x.setter
    def x(self, value: int) -> None:
        self._x = value
        self.sprite.center_x = value * TILE_SIZE

    @property
    def y(self) -> int:
        return self._y

    @y.setter
    def y(self, value: int) -> None:
        self._y = value
        self.sprite.center_y = value * TILE_SIZE

    @property
    def texture(self) -> arcade.Texture:
        return self.sprite.texture

    @texture.setter
    def texture(self, value: arcade.Texture) -> None:
        self.sprite.texture = value

    @property
    def visible(self) -> bool:
        return self.sprite.visible

    @visible.setter
    def visible(self, value: bool) -> None:
        self.sprite.visible = value

    @property
    def sprite_lists(self) -> list[arcade.SpriteList]:
        return self.sprite.sprite_lists
