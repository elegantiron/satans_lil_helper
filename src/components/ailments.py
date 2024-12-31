from __future__ import annotations
import attrs


@attrs.define
class Ailment:
    turns: int
    limit: int

@attrs.define
class Confusion(Ailment):
    pass