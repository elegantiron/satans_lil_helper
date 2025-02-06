from __future__ import annotations

from typing import TYPE_CHECKING

from sections import MainMenuSection

if TYPE_CHECKING:
    import arcade


class TestMainMenu:
    def test_creation(self, section_manager: arcade.SectionManager) -> None:
        section = MainMenuSection(0, 0, 1280, 720)
        section_manager.add_section(section)
        section.setup()
        section_manager.enable()
        section_manager.on_draw()
        section_manager.view.window.flip()
