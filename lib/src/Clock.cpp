#include "libcakes/Clock.hpp"

namespace libcakes {
    const GameTime& Clock::newFrame()
    {
        auto now            = std::chrono::steady_clock::now();
        _gameTime.lastFrame = now - _frameStart;
        _gameTime.totalElapsedTime += _gameTime.lastFrame;
        _frameStart = now;
        return _gameTime;
    }
}
