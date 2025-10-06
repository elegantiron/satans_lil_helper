#pragma once

#include <bitset>

class GameSettings {
public:
    enum class ID {
        EXIT_ON_ESCAPE,
        COUNT
    };

    void setOptionValue(ID id, bool value = true);
    bool testOptionValue(ID id);

private:
    std::bitset<static_cast<int>(ID::COUNT)> _settings;
};
