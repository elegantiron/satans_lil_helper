from __future__ import annotations

from typing import TYPE_CHECKING

import pytest

from sections import TitleSection

if TYPE_CHECKING:
    import arcade


# pylint: disable=redefined-outer-name


@pytest.fixture
def title_section(section_manager: arcade.SectionManager) -> TitleSection:  # type: ignore
    section = TitleSection(
        0, 0, int(section_manager.view.width), int(section_manager.view.height)
    )
    section_manager.add_section(section)
    section.setup()
    section_manager.enable()
    for _ in range(300):
        section.on_update(1 / 60)
    yield section  # type: ignore
    section_manager.clear_sections()


@pytest.mark.depends(on=["MessageLog", "GameWorld"], name="Title")
class TestTitle:
    def test_title_creation(self, title_section: TitleSection) -> None:
        if title_section.section_manager is None:
            raise RuntimeError
        title_section.section_manager.view.window.clear()
        title_section.on_draw()
        title_section.section_manager.view.window.flip()
        title_section.section_manager.view.window.dispatch_events()
