"""Satan's Lil Helper"""

# nuitka-project: --onefile
### nuitka-project: --standalone
# nuitka-project: --include-data-dir=assets=assets
# nuitka-project: --output-filename=slh.exe
# nuitka-project: --windows-console-mode=disable
# nuitka-project: --deployment
# nuitka-project: --output-dir=build

from __future__ import annotations

import sys
from pathlib import Path

import arcade

sys.path.insert(0, str(Path(__file__).parent / "src"))

from engine import Engine

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
