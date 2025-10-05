#include "libslh/System/Clock.hpp"

namespace libslh {
    const GameTime& Clock::newFrame()
    {
        auto now            = std::chrono::steady_clock::now();
        _gameTime.lastFrame = now - _frameStart;
        _gameTime.totalElapsedtime += _gameTime.lastFrame;
        _frameStart = now;
        return _gameTime;
    }

    const GameTime& Clock::getTime()
    {
        return _gameTime;
    }
}
