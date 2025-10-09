#pragma once

namespace SatansLilHelper {

    class Timer {
        TimerCallback _callback;
        Duration      _interval;
        Duration      _runningClock;
        bool          _running = true;

    public:
        Timer(TimerCallback callback, Duration interval);
        void update(const GameTime& gameTime);
        void update(const Duration& elapsedTime);
        void restart();
    };
}
