"""Processors to act on entities."""

from __future__ import annotations

import tcod.ecs

from components import NextAction
from exceptions import MissingComponent



class ActionProcessor:
    """Chooses and handles NPC actions, while ticking cooldowns."""
    def __init__(self, player: tcod.ecs.Entity):
        self.player = player
        player_next_action = self.player.components.get(NextAction, None)
        if player_next_action:
            self.next_player_action = player_next_action.dur
        else:
            raise MissingComponent("The Player doesn't seem to have an action timer.")

    def process(self):
        """Handle processing entities' turns.

        Processes NPC entity turns until the player's turn comes up again. It
        also iterates through every entity waiting for an action cooldown, and
        reduces that timer by 1. This happens until the player's action comes
        up again.
        
        .. todo::
           - [ ] Get the entity's FOV
           - [ ] Process NPC turns
        """

        # Check to see if the player is next and advance turns
        if self.next_player_action != 0:
            for ent in self.player.registry.Q.all_of(components=[NextAction]):
                dur = ent.components[NextAction].dur
                if dur > 0:
                    dur -= 1
                elif dur == 0 and ent is not self.player:
                    pass

class DamageStatusProcessor:
    def __init__(self):
        pass

    def process(self):
        pass