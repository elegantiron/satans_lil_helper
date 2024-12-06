from __future__ import annotations

from typing import TYPE_CHECKING

import esper

from components import NextAction, Position, Renderable
from constants import SPRITES, TILE_SIZE

if TYPE_CHECKING:
    from pygame import Surface


class RenderProcessor:
    def __init__(self, surface: Surface, sprites: dict[SPRITES, Surface]):
        self.surface = surface
        self.sprites = sprites

    def process(self):
        blitlist = []
        for ent, (pos, rend) in esper.get_components(Position, Renderable):
            blitlist.append(
                (self.sprites[rend.image], (pos.x * TILE_SIZE, pos.y * TILE_SIZE))
            )

        self.surface.blits(blitlist)


class ActionProcessor:
    def __init__(self, player):
        self.player = player

    def process(self):
        """Handle processing entities' turns.

        Processes NPC entity turns until the player's turn comes up again.
        @todo Handle enemy actions"""

        # Check to see if the player is next and advance turns
        player_action = esper.try_component(self.player, NextAction)
        player_wait = player_action.dur

        if player_wait != 0:
            for ent, act in esper.get_component(NextAction):
                if act.dur > 0:
                    act.dur -= 1
                elif act.dur == 0:
                    # Do an action here
                    pass

