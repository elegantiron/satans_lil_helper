#pragma once
#include "libslh/System/GameTime.hpp"

namespace libslh {
    using TimePoint
        = std::chrono::time_point<std::chrono::steady_clock, Duration>;

    class Clock {
        GameTime  _gameTime;
        TimePoint _frameStart{std::chrono::steady_clock::now()};

    public:
        const GameTime& newFrame();
        const GameTime& getTime();
    };
}
