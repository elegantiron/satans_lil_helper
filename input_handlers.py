from __future__ import annotations

import os

from typing import Callable, Dict, Optional, Tuple, TYPE_CHECKING, Union

from pygame import constants, Surface
from pygame.event import Event

import actions
from actions import Action, BumpAction, PickupAction, WaitAction
import color
import exceptions


if TYPE_CHECKING:
    from engine import Engine
    from entity import Item
    from pygame import freetype

MOVE_KEYS = {
    # Arrow keys.
    constants.K_UP: (0, -1),
    constants.K_DOWN: (0, 1),
    constants.K_LEFT: (-1, 0),
    constants.K_RIGHT: (1, 0),
    constants.K_HOME: (-1, -1),
    constants.K_END: (-1, 1),
    constants.K_PAGEUP: (1, -1),
    constants.K_PAGEDOWN: (1, 1),
    # Numpad keys.
    constants.K_KP_1: (-1, 1),
    constants.K_KP_2: (0, 1),
    constants.K_KP_3: (1, 1),
    constants.K_KP_4: (-1, 0),
    constants.K_KP_6: (1, 0),
    constants.K_KP_7: (-1, -1),
    constants.K_KP_8: (0, -1),
    constants.K_KP_9: (1, -1),
    # Vi keys.
    constants.K_h: (-1, 0),
    constants.K_j: (0, 1),
    constants.K_k: (0, -1),
    constants.K_l: (1, 0),
    constants.K_y: (-1, -1),
    constants.K_u: (1, -1),
    constants.K_b: (-1, 1),
    constants.K_n: (1, 1),
}

WAIT_KEYS = {
    constants.K_PERIOD,
    constants.K_KP_5,
    constants.K_CLEAR,
}

CONFIRM_KEYS = {
    constants.K_RETURN,
    constants.K_KP_ENTER,
}

ActionOrHandler = Union[Action, "BaseEventHandler"]
"""
An event handler return value which can trigger an action or switch
the active handlers.

If a handler is returned, then it will become the active handler for
future events. If an action is returned, it will be attempted and if
it is valid, then MainGameEventHandler will become the active handler.
"""


class BaseEventHandler:
    def handle_events(self, event: Event):
        """Handle an event and return the next active event handler."""
        state = self.dispatch(event)
        if isinstance(state, BaseEventHandler):
            return state
        assert not isinstance(state, Action), f"{self!r} can not handle actions."
        return self

    def on_render(
        self,
        surface: Surface,
        tile_sprites: Dict[str, Surface],
        entity_sprites: Dict[str, Surface],
        font: freetype.Font,
    ):
        raise NotImplementedError()

    def ev_keydown(self, event: Event):
        pass

    def ev_mousemotion(self, event: Event):
        pass

    def ev_mousebuttondown(self, event: Event):
        pass

    def ev_quit(self, event: Event):
        raise SystemExit()

    def dispatch(self, event: Event) -> Optional[ActionOrHandler]:
        if event.type == constants.QUIT:
            raise SystemExit()
        elif event.type == constants.KEYDOWN:
            return self.ev_keydown(event)
        elif event.type == constants.MOUSEMOTION:
            return self.ev_mousemotion(event)
        elif event.type == constants.MOUSEBUTTONDOWN:
            return self.ev_mousebuttondown(event)


class PopupMessage(BaseEventHandler):
    """Display a popup text window."""

    def __init__(self, parent_handler: BaseEventHandler, text: str):
        self.parent = parent_handler
        self.text = text

    def on_render(
        self,
        surface: Surface,
        tile_sprites: Dict[str, Surface],
        entity_sprites: Dict[str, Surface],
        font: freetype.Font,
    ):
        """Render the parent and dim the result, then print the message on top."""
        self.parent.on_render(surface, tile_sprites, entity_sprites, font)
        # TODO implement dimming the parent and displaying the popup.

    def ev_keydown(self, event: Event):
        """Any key returns to the parent handler."""
        return self.parent


class EventHandler(BaseEventHandler):
    def __init__(self, engine: Engine):
        self.engine = engine

    def handle_events(self, event: Event) -> BaseEventHandler:
        """Handle events for input handlers with an engine."""
        action_or_state = self.dispatch(event)
        if isinstance(action_or_state, BaseEventHandler):
            return action_or_state
        if self.handle_action(action_or_state):
            # A valid action was performed.
            if not self.engine.player.is_alive:
                return GameOverEventHandler(self.engine)
            elif self.engine.player.level.requires_level_up:
                return LevelUpEventHandler(self.engine)
            return MainGameEventHandler(self.engine)  # Return to the main handler.
        return self

    def handle_action(self, action: Optional[Action]) -> bool:
        """
        Handle actions returned from event methods.

        Returns True if the action will advance a turn.
        """
        if action is None:
            return False

        try:
            action.perform()
        except exceptions.Impossible as exc:
            self.engine.message_log.add_message(exc.args[0], color.impossible)
            return False  # Skip enemy turns on exceptions.

        self.engine.handle_enemy_turns()
        self.engine.handle_status_effects()
        self.engine.update_fov()
        return True

    def ev_mousemotion(self, event: Event) -> None:
        relative_pos = self.engine.game_map.convert_points(event.pos)
        if self.engine.game_map.in_bounds(*relative_pos):
            self.engine.mouse_location = relative_pos

    def on_render(
        self,
        surface: Surface,
        tile_sprites: Dict[str, Surface],
        entity_sprites: Dict[str, Surface],
        font: freetype.Font,
    ) -> None:
        self.engine.render(surface, tile_sprites, entity_sprites, font)


class AskUserEventHandler(EventHandler):
    """Handles user input for actions which require special input."""

    def ev_keydown(self, event: Event):
        """By default any key exits this input handler."""
        if event.key in {
            constants.K_LSHIFT,
            constants.K_RSHIFT,
            constants.K_LALT,
            constants.K_RALT,
            constants.K_LCTRL,
            constants.K_RCTRL,
        }:
            return None
        return self.on_exit()

    def ev_mousebuttondown(self, event: Event) -> Optional[ActionOrHandler]:
        """By default, any mouse clicks exit this input handler."""
        return self.on_exit()

    def on_exit(self) -> Optional[ActionOrHandler]:
        """
        Called when the user is trying to exit or cancel an action.

        By default this returns to the main event handler
        """
        return MainGameEventHandler(self.engine)


class CharacterScreenEventHandler(AskUserEventHandler):
    TITLE = "Character Information"

    def on_render(
        self,
        surface: Surface,
        tile_sprites: Dict[str, Surface],
        entity_sprites: Dict[str, Surface],
        font: freetype.Font,
    ) -> None:
        super().on_render(surface, tile_sprites, entity_sprites, font)

        # TODO implement rendering the character screen


class LevelUpEventHandler(AskUserEventHandler):
    TITLE = "Level Up"

    def on_render(
        self,
        surface: Surface,
        tile_sprites: Dict[str, Surface],
        entity_sprites: Dict[str, Surface],
        font: freetype.Font,
    ) -> None:
        super().on_render(surface, tile_sprites, entity_sprites, font)

        # TODO implement rendering the level up screen

    def ev_keydown(self, event: Event) -> Optional[ActionOrHandler]:
        player = self.engine.player
        key = event.key
        index = key - constants.K_a

        if 0 <= index <= 2:
            if index == 0:
                player.level.increase_max_hp()
            elif index == 1:
                player.level.increase_power()
            else:
                player.level.increase_defense()
        else:
            self.engine.message_log.add_message("Invalid entry.", color.invalid)
            return None

        return super().ev_keydown(event)

    def ev_mousebuttondown(self, event: Event) -> Optional[ActionOrHandler]:
        """
        Don't allow the player to click to exit the menu.
        """
        return None


class InventoryEventHandler(AskUserEventHandler):
    """
    This hadnler lets the user select an item.

    What happens then depends on the subclass.
    """

    TITLE = "<missing title>"

    def on_render(
        self,
        surface: Surface,
        tile_sprites: Dict[str, Surface],
        entity_sprites: Dict[str, Surface],
        font: freetype.Font,
    ) -> None:
        """
        Render an inventory menu, which displays the items in the inventory, and
        the letter to select them. Will move to a different position based on
        where the player is located, so the player can always see where they are.
        """
        super().on_render(surface, tile_sprites, entity_sprites, font)
        number_of_items_in_inventory = len(self.engine.player.inventory.items)

        height = number_of_items_in_inventory + 2

        if height <= 3:
            height = 3

        # TODO implement rendering the inventory screen box

        if number_of_items_in_inventory > 0:
            for i, item in enumerate(self.engine.player.inventory.items):
                item_key = chr(ord("a") + i)

                is_equipped = self.engine.player.equipment.item_is_equipped(item)

                item_string = f"({item_key}) {item.name}"

                if is_equipped:
                    item_string = f"{item_string} (E)"

                # TODO write `item_string` to the inventory screen box

        else:
            # TODO write "Empty" when the inventory is empty
            pass

    def ev_keydown(self, event: Event) -> Optional[ActionOrHandler]:
        player = self.engine.player
        key = event.key
        index = key - constants.K_a

        if 0 <= index <= 26:
            try:
                selected_item = player.inventory.items[index]
            except IndexError:
                self.engine.message_log.add_message("Invalid entry", color.invalid)
                return None
            return self.on_item_selected(selected_item)
        return super().ev_keydown(event)

    def on_item_selected(self, item: Item) -> Optional[ActionOrHandler]:
        """Called when the user selects a valid item."""
        raise NotImplementedError()


class InventoryActivateHandler(InventoryEventHandler):
    """Handle using an inventory item."""

    TITLE = "Select an item to use"

    def on_item_selected(self, item: Item) -> Optional[ActionOrHandler]:
        if item.consumable:
            # Return the action for the selected item.
            return item.consumable.get_action(self.engine.player)
        elif item.equippable:
            return actions.EquipAction(self.engine.player, item)
        else:
            return None


class InventoryDropHandler(InventoryEventHandler):
    """Handle dropping an inventory item"""

    TITLE = "Select an item to drop"

    def on_item_selected(self, item: Item) -> Optional[ActionOrHandler]:
        """Drop this item."""
        return actions.DropItem(self.engine.player, item)


class SelectIndexHandler(AskUserEventHandler):
    """Handles asking the user for an index on the map."""

    def __init__(self, engine: Engine):
        """Sets the cursor to the player when this handler is constructed."""
        super().__init__(engine)
        player = self.engine.player
        engine.mouse_location = player.x, player.y

    def on_render(
        self,
        surface: Surface,
        tile_sprites: Dict[str, Surface],
        entity_sprites: Dict[str, Surface],
        font: freetype.Font,
    ) -> None:
        """Highlight the tile under the cursor"""
        super().on_render(surface, tile_sprites, entity_sprites, font)
        x, y = self.engine.mouse_location
        # TODO implement rendering the mouse-based tile highlight

    def ev_keydown(self, event: Event) -> Optional[ActionOrHandler]:
        """Check for key movement or confirmation keys."""
        key = event.key

        if key in MOVE_KEYS:
            modifier = 1
            if event.mod & (constants.KMOD_LSHIFT | constants.KMOD_RSHIFT):
                modifier *= 5
            if event.mod & (constants.KMOD_LCTRL | constants.KMOD_RCTRL):
                modifier *= 10
            if event.mod & (constants.KMOD_LALT | constants.KMOD_RALT):
                modifier *= 20

            x, y = self.engine.mouse_location
            dx, dy = MOVE_KEYS[key]
            x += dx * modifier
            y += dy * modifier
            # Clamp the cursor index to the map size
            x = max(0, min(x, self.engine.game_map.width - 1))
            y = max(0, min(y, self.engine.game_map.height - 1))
            self.engine.mouse_location = x, y
            return None
        elif key in CONFIRM_KEYS:
            return self.on_index_selected(*self.engine.mouse_location)
        return super().ev_keydown(event)

    def ev_mousebuttondown(self, event: Event) -> Optional[ActionOrHandler]:
        """Left click confirms a selection."""
        if self.engine.game_map.in_bounds(*event.pos):
            if event.button == 1:
                return self.on_index_selected(*event.pos)
        return super().ev_mousebuttondown(event)

    def on_index_selected(self, x: int, y: int) -> Optional[ActionOrHandler]:
        """Called when an index is selected."""
        raise NotImplementedError()


class LookHandler(SelectIndexHandler):
    """Lets the player look around using the keyboard."""

    def on_index_selected(self, x: int, y: int) -> MainGameEventHandler:
        """Return to main event handler"""
        return MainGameEventHandler(self.engine)


class SingleRangedAttackHandler(SelectIndexHandler):
    """Handles targeting a single enemy. Only the selected enemy will be affected."""

    def __init__(
        self, engine: Engine, callback: Callable[[Tuple[int, int]], Optional[Action]]
    ):
        super().__init__(engine)

        self.callback = callback

    def on_index_selected(self, x: int, y: int) -> Optional[Action]:
        return self.callback((x, y))


class AreaRangedAttackHandler(SelectIndexHandler):
    """
    Handles targeting an area within a given radius.
    Any entity within the area will be affected.
    """

    def __init__(
        self,
        engine: Engine,
        radius: int,
        callback: Callable[[Tuple[int, int]], Optional[Action]],
    ):
        super().__init__(engine)

        self.radius = radius
        self.callback = callback

    def on_render(
        self,
        surface: Surface,
        tile_sprites: Dict[str, Surface],
        entity_sprites: Dict[str, Surface],
        font: freetype.Font,
    ) -> None:
        super().on_render(surface, tile_sprites, entity_sprites, font)

        x, y = self.engine.mouse_location

        # TODO implement rendering the targeting circle

    def on_index_selected(self, x: int, y: int) -> Optional[Action]:
        return self.callback((x, y))


class MainGameEventHandler(EventHandler):
    """
    The main handler for our game. this will handle the general input, with
    other handler for more specialized inputs.
    """

    def ev_keydown(self, event: Event) -> Optional[ActionOrHandler]:
        action: Optional[Action] = None

        key = event.key
        modifier = event.mod

        player = self.engine.player

        if key == constants.K_PERIOD and modifier & (
            constants.KMOD_LSHIFT | constants.KMOD_RSHIFT
        ):
            return actions.TakeStairsAction(player)

        if key in MOVE_KEYS:
            dx, dy = MOVE_KEYS[key]
            action = BumpAction(player, dx, dy)
        elif key in WAIT_KEYS:
            action = WaitAction(player)

        elif key == constants.K_ESCAPE:
            raise SystemExit
        elif key == constants.K_v:
            return HistoryViewer(self.engine)
        elif key == constants.K_g:
            action = PickupAction(player)
        elif key == constants.K_i:
            return InventoryActivateHandler(self.engine)
        elif key == constants.K_d:
            return InventoryDropHandler(self.engine)
        elif key == constants.K_SLASH:
            return LookHandler(self.engine)

        # No keys were pressed to change handler
        return action


class GameOverEventHandler(EventHandler):
    def on_quit(self) -> None:
        """Handle exiting out of a finished game."""
        if os.path.exists("savegame.sav"):
            os.remove("savegame.save")  # Delete the active save file
        raise exceptions.QuitWithoutSaving()  # Avoid saving a finished game

    def ev_quit(self, event: Event) -> None:
        self.on_quit()

    def ev_keydown(self, event: Event) -> None:
        if event.key == constants.K_ESCAPE:
            self.on_quit()


CURSOR_Y_KEYS = {
    constants.K_UP: -1,
    constants.K_DOWN: 1,
    constants.K_PAGEUP: -10,
    constants.K_PAGEDOWN: 10,
}


class HistoryViewer(EventHandler):
    """
    Print the history on a larger window which can be navigated.
    """

    def __init__(self, engine: Engine):
        super().__init__(engine)
        self.log_length = len(engine.message_log.messages)
        self.cursor = self.log_length - 1

    def on_render(
        self,
        surface: Surface,
        tile_sprites: Dict[str, Surface],
        entity_sprites: Dict[str, Surface],
        font: freetype.Font,
    ) -> None:
        super().on_render(surface, tile_sprites, entity_sprites, font)

        # TODO implement rendering the log history

    def ev_keydown(self, event: Event) -> Optional[MainGameEventHandler]:
        # Fancy conditional movement to make it feel right
        if event.key in CURSOR_Y_KEYS:
            adjust = CURSOR_Y_KEYS[event.key]
            if adjust < 0 and self.cursor == 0:
                # Only move from the top to the bottom when you're on the edge.
                self.cursor = self.log_length - 1
            elif adjust > 0 and self.cursor == self.log_length - 1:
                # Same with bottom to top movement.
                self.cursor = 0
            else:
                self.cursor = max(0, min(self.cursor + adjust, self.log_length - 1))
        elif event.key == constants.K_HOME:
            self.cursor == 0  # Move directly to the top message.
        elif event.key == constants.K_END:
            self.cursor == self.log_length - 1  # Move directly to the last message
        else:  # Any other key moves back to the main game state.
            return MainGameEventHandler(self.engine)
        return None
