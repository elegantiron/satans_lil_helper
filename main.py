# nuitka-project: --onefile
### nuitka-project: --standalone
# nuitka-project: --include-data-dir=assets=assets
# nuitka-project: --output-filename=slh.exe
# nuitka-project: --windows-console-mode=disable
# nuitka-project: --deployment
# nuitka-project: --output-dir=build

from __future__ import annotations

import os
from pathlib import Path

import arcade

from engine import Engine

FPS = 1 / 60


def main():
    path = Path(__file__).parent.resolve()
    arcade.resources.add_resource_handle(
        "images", os.path.join(path, "assets", "images")
    )
    arcade.resources.add_resource_handle(
        "fonts", os.path.join(path, "assets", "images")
    )
    window = arcade.Window(title="Satan's Lil Helper", draw_rate=FPS)
    window.run(Engine())


if __name__ == "__main__":
    main()
