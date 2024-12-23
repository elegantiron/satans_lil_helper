from __future__ import annotations

import PyInstaller.__main__

PyInstaller.__main__.run(
    [
        "main.py",
        "-F",
        "--add-data=assets:./assets",
        "-y",
        "--clean",
        "--log-level=WARN",
        "--hide-console=hide-early",
    ]
)
