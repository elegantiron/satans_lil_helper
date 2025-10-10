#pragma once

#include <concepts>
#include <random>

namespace SatansLilHelper::Engine {
    class RNG {
        std::mt19937_64 _rng;

    public:
        RNG(std::random_device& dev) : _rng(dev()) {}

        template <typename T>
            requires std::integral<T> || std::floating_point<T>
        T next() {
            return next<T>(std::numeric_limits<T>::min(),
                           std::numeric_limits<T>::max());
        }

        template <typename T>
            requires std::integral<T> || std::floating_point<T>
        T next(T max) {
            return next<T>(0, max);
        }

        template <typename T>
            requires std::integral<T>
        T next(T min, T max) {
            std::uniform_int_distribution<T> dist(min, max);
            return dist(_rng);
        }

        template <typename T>
            requires std::floating_point<T>
        T next(T min, T max) {
            std::uniform_real_distribution<T> dist(min, max);
            return dist(_rng);
        }
    };
}
