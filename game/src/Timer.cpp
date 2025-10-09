#include "Timer.hpp"

namespace SatansLilHelper {
    Timer::Timer(TimerCallback callback, Duration interval)
        : _callback(callback), _interval(interval), _runningClock(interval) {}

    void Timer::update(const GameTime& gameTime) {
        update(gameTime.lastFrame);
    }

    void Timer::update(const Duration& elapsedTime) {
        if (_running) {
            _runningClock -= elapsedTime;
            if (_runningClock <= Duration::zero()) {
                _callback(_running, _interval);
                _runningClock = _interval;
                if (!_running) {
                    return;
                }
            }
        }
    }

    void Timer::restart() {
        _running      = true;
        _runningClock = _interval;
    }
}
