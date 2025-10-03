#pragma once
#include <chrono>

namespace libsatan {
    using Duration = std::chrono::duration<float, std::ratio<1, 60>>;

    /**
     * @brief Contains information about how long the game has been running.
     *
     */
    struct GameTime {
        /**
         * @brief How much time has elapsed since the game started
         *
         */
        Duration totalElapsedTime;
        /**
         * @brief How long the previous frame took to calculate and render.
         *
         */
        Duration lastFrame;
    };
}
