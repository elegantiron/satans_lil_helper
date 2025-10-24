#pragma once
#include <chrono>

namespace SatansLilHelper {
    using Duration
        = std::chrono::duration<double, std::chrono::milliseconds::period>;
    using TimePoint
        = std::chrono::time_point<std::chrono::steady_clock, Duration>;
}
