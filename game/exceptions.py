class Impossible(Exception):
    """Raised for actions that are impossible."""

class InventoryFull(Impossible):
    """Raised when the inventory is full."""
    def __init__(self, message):
        super().__init__(message if message else "Your inventory is full.")

class MissingComponent(Impossible):
    """Raised if an entity is missing a component."""

class PathBlocked(Impossible):
    """Raised when the path is blocked in a direction."""
    def __init__(self, message):
        super().__init__(message if message else "That way is blocked.")