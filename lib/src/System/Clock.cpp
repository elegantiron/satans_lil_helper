#include "libsatan/System/Clock.hpp"

#include <chrono>

namespace libsatan::System {
    void Clock::update()
    {
        auto now            = std::chrono::steady_clock::now();
        _gameTime.lastFrame = now - _frameStart;
        _gameTime.totalElapsedTime += _gameTime.lastFrame;
        _frameStart = now;
    }

    const GameTime& Clock::getTime()
    {
        return _gameTime;
    }
}
