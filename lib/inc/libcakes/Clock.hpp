#pragma once
#include "libcakes/GameTime.hpp"
namespace libcakes {
    using TimePoint
        = std::chrono::time_point<std::chrono::steady_clock, Duration>;

    /**
     * @brief Measures time for a game.
     *
     */
    class Clock {
        GameTime  _gameTime;
        TimePoint _frameStart;

    public:
        /**
         * @brief Indicates the start of a new frame.
         * @details Calculates the length of the previous frame and how long it
         * has been since the start of the game.
         *
         * @return const GameTime&
         */
        const GameTime& newFrame();
    };
}
