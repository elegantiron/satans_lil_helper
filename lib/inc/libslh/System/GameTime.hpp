#pragma once
#include <chrono>

namespace {
    inline constexpr int MillisPerSecond = 1000;
}

namespace libslh {
    using Duration
        = std::chrono::duration<long double, std::ratio<1, MillisPerSecond>>;

    struct GameTime {
        Duration totalElapsedtime{0};
        Duration lastFrame{0};
    };
}
