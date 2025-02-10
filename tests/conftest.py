from __future__ import annotations

import contextlib
import os
from pathlib import Path
from typing import TYPE_CHECKING, Final

import arcade
import pytest
from arcade import gl

from components import Inventory, Position
from engine import Engine
from entities import consumables, enemies
from gameworld import GameWorld
from messagelog import MessageLog

if TYPE_CHECKING:
    from collections.abc import Iterator

    from tcod.ecs import Entity

# pylint: disable=redefined-outer-name
PROJECT_ROOT = (Path(__file__).parent.parent).resolve()
FIXTURE_ROOT = PROJECT_ROOT / "tests" / "fixtures"
REAL_WINDOW_CLASS = arcade.Window  # pylint: disable=invalid-name
WINDOW: arcade.Window | None = None
OFFSCREEN: arcade.Window | None = None

arcade.resources.add_resource_handle("images", PROJECT_ROOT / "assets" / "images")
arcade.resources.add_resource_handle("fonts", PROJECT_ROOT / "assets" / "fonts")

SEED: Final[float] = 1737855529.0953882


@pytest.fixture
def message_log() -> MessageLog:
    return MessageLog()


@pytest.fixture
def item(gameworld_fixed_seed: GameWorld) -> Entity:
    return gameworld_fixed_seed.spawn_entity(
        consumables.health_potion,
        (
            gameworld_fixed_seed.player.components[Position].x,
            gameworld_fixed_seed.player.components[Position].y,
        ),
    )


@pytest.fixture
def entity_no_inventory(gameworld_fixed_seed: GameWorld) -> Iterator[Entity]:
    new_entity = gameworld_fixed_seed.registry.new_entity()
    new_entity.components[Position] = Position(
        x=gameworld_fixed_seed.player.components[Position].x,
        y=gameworld_fixed_seed.player.components[Position].y,
    )
    new_entity.components[Inventory] = Inventory()
    yield new_entity
    new_entity.clear()


@pytest.fixture(scope="class")
def gameworld_fixed_seed() -> GameWorld:
    return GameWorld(SEED)


@pytest.fixture
def gameworld_random_seed() -> GameWorld:
    return GameWorld()


@pytest.fixture
def gameworld_random_seed2() -> GameWorld:
    return GameWorld()


@pytest.fixture
def wolf_one_below(gameworld_fixed_seed: GameWorld) -> Iterator[Entity]:
    nentity = gameworld_fixed_seed.spawn_entity(
        enemies.forest.wolf,
        (
            gameworld_fixed_seed.player.components[Position].x,
            gameworld_fixed_seed.player.components[Position].y - 1,
        ),
    )
    yield nentity
    nentity.clear()


@pytest.fixture
def wolf_one_above(gameworld_fixed_seed: GameWorld) -> Iterator[Entity]:
    nentity = gameworld_fixed_seed.spawn_entity(
        enemies.forest.wolf,
        (
            gameworld_fixed_seed.player.components[Position].x,
            gameworld_fixed_seed.player.components[Position].y + 1,
        ),
    )
    yield nentity
    nentity.clear()


@pytest.fixture
def strong_entity(gameworld_fixed_seed: GameWorld) -> Iterator[Entity]:
    sentity = gameworld_fixed_seed.spawn_entity(
        enemies.testing.strong,
        (
            gameworld_fixed_seed.player.components[Position].x,
            gameworld_fixed_seed.player.components[Position].y - 1,
        ),
    )
    yield sentity
    sentity.clear()


@pytest.fixture
def weak_entity(gameworld_fixed_seed: GameWorld) -> Entity:  # type: ignore
    wentity = gameworld_fixed_seed.spawn_entity(
        enemies.testing.weak,
        (
            gameworld_fixed_seed.player.components[Position].x,
            gameworld_fixed_seed.player.components[Position].y - 1,
        ),
    )
    yield wentity  # type: ignore
    wentity.clear()


def make_window_caption(
    request: pytest.FixtureRequest | None = None,
    prefix: str = "Testing",
    sep: str = " - ",
) -> str:
    """Centralizes test name customization.

    It helps with:

    1. Tests scoped as something other than function (can't use test_name fixture)
    2. Local (non-CI) temp modifications of inter-test behavior
    """
    parts = [prefix]
    if request is not None:
        parts.append(request.node.name)

    return sep.join(parts)


def create_window(width=1280, height=720, caption="Testing", **kwargs):
    global WINDOW  # noqa: PLW0603  # pylint: disable=global-statement
    if not WINDOW:
        WINDOW = REAL_WINDOW_CLASS(
            width=width, height=height, title=caption, vsync=False, antialiasing=False
        )
        WINDOW.set_vsync(False)
        # This value is being monkey-patched into the Window class so that tests can identify
        # if we are using arcade-accelerate easily in case they need to disable something when
        # it is enabled.
        WINDOW.using_accelerate = os.environ.get("ARCADE_PYTEST_USE_RUST")  # type: ignore
    return WINDOW


def prepare_window(window: arcade.Window, caption: str | None = None) -> None:
    # Check if someone has been naughty
    if window.has_exit:
        raise RuntimeError("Please do not close the global test window :D")

    window.switch_to()
    if window.get_size() < (1280, 720):
        window.set_size(1280, 720)
    if caption:
        window.set_caption(caption)

    ctx = window.ctx
    arcade.SpriteList.DEFAULT_TEXTURE_FILTER = gl.LINEAR, gl.LINEAR
    window._start_finish_render_data = None  # noqa: SLF001 # pylint: disable=protected-access
    window.hide_view()  # Disable views if any is active
    window.dispatch_pending_events()
    with contextlib.suppress(Exception):
        arcade.disable_timings()

    # Reset context (various states)
    ctx.reset()
    window.set_vsync(False)
    window.flip()
    window.clear()
    window.default_camera.use()
    ctx.gc_mode = "context_gc"
    ctx.gc()

    # Ensure no old functions are lingering
    window.on_draw = lambda: None
    window.on_update = lambda delta_time: None


@pytest.fixture
def test_name(request):
    return make_window_caption(request)


@pytest.fixture
def ctx(test_name):
    """
    Per function context.

    The main purpose of this is to ensure that the context is reset
    between each test function and the window is flipped.
    """
    window = create_window()
    arcade.set_window(window)
    prepare_window(window, caption=test_name)
    return window.ctx


@pytest.fixture(scope="session")
def ctx_static(request):
    """
    Context that is shared between tests
    This is the same global context.
    Module scoped fixtures can inject this context.
    """
    window = create_window()
    arcade.set_window(window)
    # Can't use the test_name fixture here:
    # 1. This fixture is session scoped
    # 2. test_name is function scoped
    prepare_window(window, caption=make_window_caption(request))
    return window.ctx


@pytest.fixture
def window(test_name):
    """
    Global window that is shared between tests.

    This just returns the global window, but ensures that the context
    is reset between each test function and the window is flipped
    between each test function.
    """
    window = create_window()
    arcade.set_window(window)
    prepare_window(window, caption=test_name)
    return window


class TestView(arcade.View):
    def __init__(
        self,
        window: arcade.Window | None = None,
        background_color: tuple[int, int, int]
        | tuple[int, int, int, int]
        | None = None,
    ) -> None:
        super().__init__(window, background_color)
        self.satan_sprites = arcade.SpriteList()
        self.satan = {
            "main": arcade.Sprite(
                ":images:satan/main.png", 1, self.width // 2, self.height // 2
            ),
            "eyes open": arcade.Sprite(
                ":images:satan/eyes_open.png",
                1,
                self.width / 2,
                self.height / 2,
            ),
            "mouth_closed": arcade.Sprite(
                ":images:satan/mouth_closed.png",
                1,
                self.width / 2,
                self.height / 2,
            ),
            "eyes closed": arcade.Sprite(
                ":images:satan/eyes_closed.png",
                1,
                self.width / 2,
                self.height / 2,
            ),
        }
        self.satan["eyes closed"].visible = False

        self.satan_sprites.append(self.satan["main"])
        self.satan_sprites.append(self.satan["mouth_closed"])
        self.satan_sprites.append(self.satan["eyes open"])
        self.satan_sprites.append(self.satan["eyes closed"])


@pytest.fixture
def view(window: arcade.Window) -> arcade.View:
    new_view = TestView(window)
    window.show_view(new_view)
    return new_view


@pytest.fixture
def section_manager(view: arcade.View) -> arcade.SectionManager:
    return arcade.SectionManager(view)


@pytest.fixture
def engine(window: arcade.Window) -> Engine:
    return Engine(window)
