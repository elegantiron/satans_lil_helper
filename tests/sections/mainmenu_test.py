from __future__ import annotations

from typing import TYPE_CHECKING

import pytest

from sections import MainMenuSection

if TYPE_CHECKING:
    import arcade


# pylint: disable=redefined-outer-name


@pytest.fixture
def menu_section(section_manager: arcade.SectionManager) -> MainMenuSection: # type: ignore
    section = MainMenuSection(
        0, 0, int(section_manager.view.width), int(section_manager.view.height)
    )
    section_manager.add_section(section)
    section.setup()
    section_manager.enable()
    for _ in range(300):
        section.on_update(1 / 60)
    yield section # type: ignore
    section_manager.clear_sections()


@pytest.mark.depends(on=["MessageLog", "GameWorld"], name="MainMenu")
@pytest.mark.gui
class TestMainMenu:
    def test_main_menu_creation(self, menu_section: MainMenuSection) -> None:
        if menu_section.section_manager is None:
            raise RuntimeError
        menu_section.section_manager.view.window.clear()
        menu_section.on_draw()
        menu_section.section_manager.view.window.flip()
        menu_section.section_manager.view.window.dispatch_events()
