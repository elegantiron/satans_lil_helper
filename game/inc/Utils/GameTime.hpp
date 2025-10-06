#pragma once
#include <chrono>

namespace {
inline constexpr int MILLIS_PER_SECOND = 1000;
}

using Duration
    = std::chrono::duration<long double, std::ratio<1, MILLIS_PER_SECOND>>;

struct GameTime {
    Duration totalElapsedTime;
    Duration lastFrame;
};
