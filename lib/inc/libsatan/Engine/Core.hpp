#ifndef LIBSATAN_ENGINE_CORE_HPP
#define LIBSATAN_ENGINE_CORE_HPP
#include "libsatan/Engine/Scene.hpp"
#include "libsatan/Engine/SceneStack.hpp"
#include "libsatan/System/Clock.hpp"

#include <SFML/Graphics/Color.hpp>
#include <SFML/Graphics/RenderWindow.hpp>
#include <SFML/System/Vector2.hpp>
#include <bitset>
#include <memory>

namespace {
    inline constexpr int SETTINGS_BITS{10};
}

namespace libsatan::Engine {
    using namespace System;
    class Game {
        enum Setting
        {
            IMMEDIATE_SCENE_TRANSITION,
            SCENE_MULTISTACK
        };
        Game() = default;
        static std::unique_ptr<Game> _instance;
        ScenePtr                     _nextScene;
        sf::RenderWindow             _window;
        std::bitset<SETTINGS_BITS>   _settings{0};
        Clock                        _clock;
        SceneStack                   _scenes;

    public:
        static Game& getInstance();

        void run(sf::Vector2u windowSize,
                 sf::String   windowTitle,
                 ScenePtr     pScene = nullptr);
        void setNextScene(ScenePtr pScene);
        void setBackgroundColor(sf::Color color);

        // Settings control methods
        void setImmediateSceneTransferEnabled(bool enabled);
        void setSceneMultiStackEnabled(bool enabled);

    private:
        void dumpCore();
        void gameLoop();
    };
} // namespace libsatan::Engine

#endif
