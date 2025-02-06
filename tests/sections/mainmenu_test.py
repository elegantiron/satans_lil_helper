from __future__ import annotations

from typing import TYPE_CHECKING

import pytest

from sections import MainMenuSection

if TYPE_CHECKING:
    import arcade


# pylint: disable=redefined-outer-name


@pytest.fixture
def menu_section(section_manager: arcade.SectionManager) -> MainMenuSection:
    section = MainMenuSection(
        0, 0, int(section_manager.view.width), int(section_manager.view.height)
    )
    section_manager.add_section(section)
    section.setup()
    section_manager.enable()
    for _ in range(300):
        section.on_update(1/60)
    return section


class TestMainMenu:
    def test_creation(self, menu_section: MainMenuSection) -> None:
        menu_section.on_draw()
        if menu_section.section_manager is not None:
            menu_section.section_manager.view.window.flip()
