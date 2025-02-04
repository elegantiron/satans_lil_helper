"""Position component"""

from __future__ import annotations

import arcade

from constants import TILE_SIZE


class Position:
    """An entity's position and sprite"""

    def __init__(self, *, x: int = 0, y: int = 0, sprite: str | None = None) -> None:
        self._x = x
        self._y = y
        if sprite is not None:
            self.sprite = arcade.Sprite(sprite)
        else:
            self.sprite = arcade.Sprite(
                center_x=self._x * TILE_SIZE, center_y=self._y * TILE_SIZE
            )
        self.sprite.center_x = x * TILE_SIZE
        self.sprite.center_y = y * TILE_SIZE

    @property
    def x(self) -> int:
        """Grid x coordinate"""
        return self._x

    @x.setter
    def x(self, value: int) -> None:
        self._x = value
        self.sprite.center_x = value * TILE_SIZE

    @property
    def y(self) -> int:
        """Grid y coordinate"""
        return self._y

    @y.setter
    def y(self, value: int) -> None:
        self._y = value
        self.sprite.center_y = value * TILE_SIZE

    @property
    def texture(self) -> arcade.Texture:
        """Entity's texture"""
        return self.sprite.texture

    @texture.setter
    def texture(self, value: arcade.Texture) -> None:
        self.sprite.texture = value

    @property
    def visible(self) -> bool:
        """Whether the entity is visible"""
        return self.sprite.visible

    @visible.setter
    def visible(self, value: bool) -> None:
        self.sprite.visible = value

    @property
    def sprite_lists(self) -> list[arcade.SpriteList]:
        """Which SpriteLists this entity is in"""
        return self.sprite.sprite_lists

    @property
    def xy(self) -> tuple[int, int]:
        """The coordinates as a tuple"""
        return self.x, self.y
