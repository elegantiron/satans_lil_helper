from __future__ import annotations

from typing import Tuple, TYPE_CHECKING

from pygame import draw, freetype
from constants import STATUS
import color

if TYPE_CHECKING:
    from engine import Engine
    from game_map import GameMap
    from pygame import Surface
    from entity import Actor


def get_names_at_location(x: int, y: int, game_map: GameMap) -> str:
    if not game_map.in_bounds(x, y) or not game_map.visible[x, y]:
        return ""

    names = ", ".join(
        entity.name for entity in game_map.entities if entity.x == x and entity.y == y
    )

    return names


def render_bar(
    surface: Surface, current_value: int, maximum_value: int, total_width: int
) -> None:
    bar_width = int(float(current_value) / maximum_value * total_width)
    draw.rect(
        surface=surface,
        color=(0xFF, 0x00, 0x00),
        rect=(
            int(STATUS.WIDTH * 2.1),
            int(STATUS.HEIGHT * 0.9),
            total_width,
            int(STATUS.HEIGHT * 0.02),
        ),
    )
    draw.rect(
        surface=surface,
        color=(0x00, 0xFF, 0x00),
        rect=(
            int(STATUS.WIDTH * 2.1),
            int(STATUS.HEIGHT * 0.9),
            bar_width,
            int(STATUS.HEIGHT * 0.02),
        ),
    )


def render_game_status(
    surface: Surface,
    dungeon_level: int,
    font: freetype.Font,
    player: Actor,
) -> None:
    """
    Render information about the current game state which the
    player might find useful.
    """
    textlist = [
        f"Position: {player.x},{player.y}",
        f"Area: {dungeon_level}",
        "",
    ]

    health = f"Health: {player.fighter.hp}/{player.fighter.max_hp}"
    mana = f"Mana: {player.fighter.mana}/{player.fighter.max_mana}"
    experience = f"Experience: {player.level.current_xp}"
    next_level = f"Next level: {player.level.experience_to_next_level}"
    textlist.append("Status:")
    textlist.append(health)
    textlist.append(mana)
    textlist.append(experience)
    textlist.append(next_level)
    textlist.append("")

    armor = "Armor:  "
    weapon = "Weapon: "
    try:
        armor = f"{armor}{player.equipment.armor.name}"
    except AttributeError:
        pass
    try:
        weapon = f"{weapon}{player.equipment.weapon.name}"
    except AttributeError:
        pass
    textlist.append("Equipment: ")
    textlist.append(weapon)
    textlist.append(armor)

    position = (int(STATUS.WIDTH * 2.1), int(STATUS.HEIGHT * 0.01))
    for text in textlist:
        position = render_line(
            text=text,
            font=font,
            surface=surface,
            position=position,
            fgcolor=(0xFF, 0xFF, 0xFF),
            bgcolor=(0x00, 0x00, 0x00),
        )


def render_line(
    *,
    text: str,
    font: freetype.Font,
    surface: Surface,
    position: Tuple[int, int],
    fgcolor: Tuple[int, int, int] = color.white,
    bgcolor: Tuple[int, int, int] = color.black,
) -> Tuple[int, int]:
    box = font.get_rect(text)
    font.render_to(
        surf=surface,
        dest=position,
        text=text,
        fgcolor=fgcolor,
        bgcolor=bgcolor,
    )
    position = (position[0], position[1] + box[1] + 4)
    return position


def render_names_at_mouse_location(
    surface: Surface, font: freetype.Font, x: int, y: int, engine: Engine
) -> None:
    mouse_x, mouse_y = engine.mouse_location

    names_at_mouse_location = get_names_at_location(
        x=mouse_x, y=mouse_y, game_map=engine.game_map
    )
    if names_at_mouse_location != "":
        text = f"Here you spy: {names_at_mouse_location}"
        
        font.render_to(
            surf=surface,
            dest=(x, y),
            text=text,
            fgcolor=(0xFF, 0xFF, 0xFF),
            bgcolor=(0, 0, 0, 0x80),
        )
