from __future__ import annotations

import PyInstaller.__main__

PyInstaller.__main__.run(
    [
        "main.py",
        "-F",
        "--add-data=assets:./assets",
        "-y",
        "--log-level=WARN",
        "--hide-console=hide-early",
        "--workpath=./build/.build",
        "--distpath=./build/dist",
        "--name=slh",
    ]
)
