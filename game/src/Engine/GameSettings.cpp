#include "Engine/GameSettings.hpp"

void GameSettings::setOptionValue(ID id, bool value) {
    _settings.set(static_cast<int>(id), value);
}

bool GameSettings::testOptionValue(ID id) {
    return _settings.test(static_cast<int>(id));
}
