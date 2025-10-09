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
        void update(const GameTime& gameTime);
        void update(const Duration& elapsedTime);
        void restart();
        void setInterval(Duration interval);

        Timer(T* object, TimerCallback<T> callback);
        void operator=(const Timer&)  = delete;
        void operator=(const Timer&&) = delete;
        Timer(Timer&)                 = delete;
        Timer(Timer&&)                = delete;
        ~Timer()                      = default;
    };
}
