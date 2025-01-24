from __future__ import annotations

from pathlib import Path
from typing import TYPE_CHECKING

import arcade
import pytest

import bestiary
import gameworld
import utils

if TYPE_CHECKING:
    import pathlib


path = Path(__file__).parent.parent.resolve()
arcade.resources.add_resource_handle("images", path / "assets" / "images")
arcade.resources.add_resource_handle("fonts", path / "assets" / "fonts")

@pytest.mark.parametrize(
    ("pixel_address", "grid_address"),
    [
        ((128, 128), (4, 4)),
        ((256, 128), (8, 4)),
        ((32, 0), (1, 0)),
        ((512, 288), (16, 9)),
    ],
)
class TestConverters:
    def test_px_to_grid(self, pixel_address, grid_address):
        assert utils.pixel_to_grid(pixel_address) == grid_address

    def test_grid_to_px(self, grid_address, pixel_address):
        assert utils.grid_to_pixel(grid_address) == pixel_address


@pytest.fixture(scope="class")
def save_dir(tmp_path_factory: pytest.TempPathFactory) -> pathlib.Path:
    return tmp_path_factory.mktemp("data") / "saves"


SAVE_DATA = b"sadhjafeuwaihroldsbkjhlahufeidajbnk"


@pytest.mark.parametrize(
    ("test_data"),
    [
        pytest.param(bestiary.Bestiary(), id="bestiary"),
        pytest.param(gameworld.GameWorld(), id="game world"),
    ],
)
def test_save_load(save_dir, test_data):
    utils.save_data(test_data, save_dir)
    loaded_data = utils.load_data(save_dir)
    assert loaded_data == test_data
