#include "libslh/Engine/Core.hpp"

#include <utility>

namespace libslh::Engine {
    Core* Core::_instance{nullptr};

    Core& Core::getInstance() {
        if (_instance == nullptr) {
            _instance = new Core;
        }
        return *_instance;
    }

    void Core::run(const char*  title,
                   sf::Vector2u windowSize,
                   ScenePtr     startingScene) {
        _window.create(sf::VideoMode(windowSize), title);
        _sceneMan.init(std::move(startingScene));
        bool iterateAgain = true;
        while (iterateAgain) {
            drawGame();
            gameLoop(iterateAgain);
        }
    }

    void Core::gameLoop(bool& iterateAgain) {
        iterateAgain          = true;
        bool updateSuccessful = true;
        _sceneMan.updateScene(_clock.newFrame(), updateSuccessful);
        if (!updateSuccessful) {
            iterateAgain = false;
            return;
        }
        processEvents();
        if (!_sceneMan.hasScenes()) {
            iterateAgain = false;
            return;
        }
    }

    void Core::processEvents() {
        bool handledSuccessfully = true;
        while (std::optional event = _window.pollEvent()) {
            _sceneMan.dispatchEvent(event, handledSuccessfully);
            if (!handledSuccessfully) {
                return;
            }
        }
    }

    sf::Vector2u Core::getWindowSize() const {
        return _window.getSize();
    }

    void Core::drawGame() {
        _window.clear();
        _window.draw(_sceneMan);
        _window.display();
    }
} // namespace libslh::Engine
