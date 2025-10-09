#include "Timer.hpp"

namespace SatansLilHelper {

    template <typename T>
    void Timer<T>::setInterval(Duration interval) {
        _interval = _runningClock = interval;
    }

    template <typename T>
    void Timer<T>::update(const GameTime& gameTime) {
        update(gameTime.lastFrame);
    }

    template <typename T>
    void Timer<T>::update(const Duration& elapsedTime) {
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

    template <typename T>
    Timer<T>::Timer(T* object, TimerCallback<T> callback)
        : _object(object), _callback(callback) {}

}
