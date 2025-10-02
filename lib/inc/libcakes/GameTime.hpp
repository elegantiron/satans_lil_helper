#pragma once
#include <chrono>

namespace libcakes {
    using Duration = std::chrono::duration<float, std::ratio<1, 60>>;

    struct GameTime {
        Duration totalElapsedTime;
        Duration lastFrame;
    };
}
