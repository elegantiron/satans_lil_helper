"""Game sections"""

from .bestiarysection import BestiarySection
from .gamemapsection import GameMapSection
from .inspectorsection import InspectorSection
from .inventorysection import InventorySection
from .mainmenusection import MainMenuSection
from .messagesection import MessageSection
from .pausemenusection import PauseSection
from .statussection import StatusSection
from .titlesection import TitleSection

__all__ = [
    "TitleSection",
    "MainMenuSection",
    "BestiarySection",
    "GameMapSection",
    "StatusSection",
    "MessageSection",
    "PauseSection",
    "InspectorSection",
    "InventorySection",
]
