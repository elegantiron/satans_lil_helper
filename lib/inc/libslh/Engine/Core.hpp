#pragma once

#include "libslh/Engine/SceneManager.hpp"
#include "libslh/System/Clock.hpp"

#include <SFML/Graphics/RenderWindow.hpp>

namespace libslh::Engine {

    class Core {
        static Core* _instance;
        Core() = default;
        ~Core();

    public:
        static Core& getInstance();
        Core&        operator=(const Core&) = delete;
        Core&        operator=(Core&&)      = delete;
        Core(Core&)                         = delete;
        Core(Core&&)                        = delete;

    private:
        Clock            _clock;
        SceneManager     _sceneMan;
        ScenePtr         _nextScene;
        sf::RenderWindow _window;

        void init();
        void gameLoop(bool& iterateAgain);
        void processEvents();
        void drawGame();

    public:
        void         run(const char*  title,
                         sf::Vector2u windowSize,
                         ScenePtr     startingScene);
        void         setNextScene(ScenePtr pScene);
        sf::Vector2u getWindowSize() const;
    };
}
