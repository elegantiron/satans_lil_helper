"""Satan's Lil Helper"""

from __future__ import annotations

import sys
from pathlib import Path

import arcade

sys.path.insert(0, str(Path(__file__).parent / "src"))

from engine import Engine  # pylint: disable=wrong-import-position

FPS = 1 / 60


def main() -> None:
    """Main function"""
    path = Path(__file__).parent.resolve()
    arcade.resources.add_resource_handle("images", Path(path) / "assets" / "images")
    arcade.resources.add_resource_handle("fonts", Path(path) / "assets" / "fonts")
    window = arcade.Window(title="Satan's Lil Helper", draw_rate=FPS)
    window.run(Engine())


if __name__ == "__main__":
    main()
