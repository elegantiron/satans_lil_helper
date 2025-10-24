#pragma once

#include "System/Time.hpp"

namespace SatansLilHelper {
    using namespace std::chrono_literals;

    class GameTime {
        Duration  _lastFrame        = 0s;
        Duration  _totalElapsedTime = 0s;
        TimePoint _lastUpdate       = std::chrono::steady_clock::now();

    public:
        GameTime()  = default;
        ~GameTime() = default;

        void update();
        [[nodiscard]]
        Duration getLastFrame() const;
        [[nodiscard]]
        Duration getTotalElapsedTime() const;
    };
}
