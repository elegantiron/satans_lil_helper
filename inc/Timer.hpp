#pragma once

namespace SatansLilHelper {
    template <typename T>
    class Timer {
        T*               _object;
        TimerCallback<T> _callback;
        Duration         _interval{0};
        Duration         _runningClock{0};
        bool             _running = true;

    public:
        // Timer(TimerCallback<T> callback, Duration interval);
        void update(const GameTime& gameTime) {
            update(gameTime.lastFrame);
        }

        void update(const Duration& elapsedTime) {
            if (_running) {
                _runningClock -= elapsedTime;
                if (_runningClock <= Duration::zero()) {
                    (_object->*_callback)(_running, _interval);
                    _runningClock = _interval;
                    if (!_running) {
                        return;
                    }
                }
            }
        }

        void restart();

        void setInterval(Duration interval) {
            _interval = _runningClock = interval;
        }

        Timer(T* object, TimerCallback<T> callback)
            : _object(object), _callback(callback) {}

        void operator=(const Timer&)  = delete;
        void operator=(const Timer&&) = delete;
        Timer(Timer&)                 = delete;
        Timer(Timer&&)                = delete;
        ~Timer()                      = default;
    };
} // namespace SatansLilHelper
