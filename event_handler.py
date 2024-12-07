from typing import Any, Callable, TypeVar
import warnings

import pygame


T = TypeVar("T")
class EventDispatcher:
    def dispatch(self, event: pygame.Event) -> T | None:
        if event.type is None:
            warnings.warn(
                "`event.type` attribute should not be None.",
                DeprecationWarning,
                stacklevel=2,
            )
            return None
        func_name = f"ev_{pygame.event.event_name(event.type).lower()}"
        func: Callable[[Any], T | None] | None = getattr(self, func_name, None)
        if func is None:
            warnings.warn(
                f"{func_name} is missing from this EventDispatcher object.",
                RuntimeWarning,
                stacklevel=2,
            )
            return None
        return func(event)

    def ev_keydown(self, event):
        pass

    def ev_keyup(self, event):
        pass

    def ev_mousemotion(self, event):
        pass

    def ev_mousebuttondown(self, event):
        pass

    def ev_mousebuttonup(self, event):
        pass

    def ev_joyaxismotion(self, event):
        pass

    def ev_joyballmotion(self, event):
        pass

    def ev_joyhatmotion(self, event):
        pass

    def ev_joybuttondown(self, event):
        pass

    def ev_joybuttonup(self, event):
        pass

    def ev_videoresize(self, event):
        pass

    def ev_videoexpose(self, event):
        pass

    def ev_userevent(self, event):
        pass

    def ev_audiodeviceadded(self, event):
        pass

    def ev_audiodeviceremoved(self, event):
        pass

    def ev_fingermotion(self, event):
        pass

    def ev_fingerdown(self, event):
        pass

    def ev_fingerup(self, event):
        pass

    def ev_mousewheel(self, event):
        pass

    def ev_quit(self, event):
        pass

    def ev_windowshown(self, event):
        pass

    def ev_activeevent(self, event):
        pass

    def ev_windowfocusgained(self, event):
        pass

    def ev_textediting(self, event):
        pass

    def ev_windowenter(self, event):
        pass

    def ev_windowexposed(self, event):
        pass

    def ev_windowclose(self, event):
        pass

    def ev_windowleave(self, event):
        pass