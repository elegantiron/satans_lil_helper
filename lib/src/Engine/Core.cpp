#include "libsatan/Engine/Core.hpp"

#include <SFML/Window/VideoMode.hpp>
#include <iostream>

namespace libsatan::Engine {
    std::unique_ptr<Core> Core::_instance{nullptr};

    Core& Core::getInstance()
    {
        if (_instance == nullptr) {
            _instance.reset(new Core);
        }
        return *_instance;
    }

    void Core::run(sf::Vector2u windowSize,
                   sf::String   windowTitle,
                   ScenePtr     pScene)
    {
        _window.create(sf::VideoMode(windowSize), windowTitle);
        if (_nextScene != nullptr) {
            transitionScene();
        }
        if (pScene != nullptr) {
            _nextScene = pScene;
            transitionScene();
        }
        if (_scenes.empty()) {
            std::cout << "You can't have a game with no scenes!" << std::endl;
            return;
        }
        gameLoop();
        dumpCore();
    }

    void Core::transitionScene()
    {
        if (!_scenes.empty()) {
            _scenes.top()->onBury();
        }
        _nextScene->init();
        _scenes.push(_nextScene);
        _nextScene = nullptr;
    }

    void Core::dumpCore()
    {
        while (!_scenes.empty()) {
            _scenes.pop();
        }
    }

    void Core::gameLoop(){
        // switch(_scenes.top()->update(_clock.getTime())){}
    }
} // namespace libsatan::Engine
