#ifndef LIBSATAN_ENGINE_CORE_HPP
#define LIBSATAN_ENGINE_CORE_HPP
#include "libsatan/Engine/Scene.hpp"
#include "libsatan/System/Clock.hpp"

#include <SFML/Graphics/Color.hpp>
#include <SFML/Graphics/RenderWindow.hpp>
#include <SFML/System/Vector2.hpp>
#include <bitset>
#include <memory>
#include <stack>

namespace {
    inline constexpr int SETTINGS_BITS{10};
}

namespace libsatan::Engine {
    using namespace System;
    class Core {
        Core() = default;
        static std::unique_ptr<Core> _instance;
        ScenePtr                     _nextScene;
        std::stack<ScenePtr>         _scenes;
        sf::RenderWindow             _window;
        std::bitset<SETTINGS_BITS>   _settings{0};
        Clock                        _clock;

    public:
        static Core& getInstance();

        void run(sf::Vector2u windowSize,
                 sf::String   windowTitle,
                 ScenePtr     pScene = nullptr);
        void setNextScene(ScenePtr pScene);
        void setBackgroundColor(sf::Color color);

    private:
        void transitionScene();
        void popScene();
        void dumpCore();
        void gameLoop();
    };
}

#endif
