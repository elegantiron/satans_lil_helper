#pragma once
#include "libsatan/System/GameTime.hpp"

#include <chrono>

namespace libsatan::System {
    class Clock {
        GameTime _gameTime{};
        std::chrono::time_point<std::chrono::steady_clock, Duration>
            _frameStart{std::chrono::steady_clock::now()};

    public:
        void            update();
        const GameTime& getTime();
    };
}
