#pragma once

#include "GameTime.hpp"

#include <chrono>

namespace SatansLilHelper {
    using TimePoint
        = std::chrono::time_point<std::chrono::steady_clock, Duration>;

    class Clock {
        GameTime  _gameTime;
        TimePoint _frameStart{std::chrono::steady_clock::now()};

    public:
        const GameTime& newFrame();
        [[nodiscard]]
        const GameTime& getTime() const;
    };
}
