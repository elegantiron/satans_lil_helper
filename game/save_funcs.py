from __future__ import annotations
import hashlib
import hmac
import lzma

import dill as pickle

from game import input_handlers
from game.constants import HMAC_KEY
from game.exceptions import Haxx0red


def save_data(data, filename):
    if isinstance(data, input_handlers.GameMenuInputHandler):
        data = data._parent
    raw_data = pickle.dumps(data)
    save_data = lzma.compress(raw_data)
    signer = hmac.new(HMAC_KEY, digestmod=hashlib.blake2b)
    signer.update(save_data)
    mac_result = signer.digest()
    with open(filename, "wb") as f:
        f.write(mac_result)
    with open(filename, "ab") as f:
        f.write(save_data)


def load_data(filename):
    signer = hmac.new(HMAC_KEY, digestmod=hashlib.blake2b)
    save_data = ""
    try:
        with open(filename, "rb") as f:
            mac_data = f.read(signer.digest_size)
            save_data = f.read()
        signer.update(save_data)
        computed_mac = signer.digest()
        if hmac.compare_digest(mac_data, computed_mac):
            return pickle.loads(lzma.decompress(save_data))
        else:
            raise Haxx0red
    except FileNotFoundError:
        return None
