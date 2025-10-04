#include "libsatan/Engine/Core.hpp"

#include <SFML/Window/VideoMode.hpp>

namespace libsatan::Engine {
    std::unique_ptr<Game> Game::_instance{nullptr};

    Game& Game::getInstance()
    {
        if (_instance == nullptr) {
            _instance.reset(new Game);
        }
        return *_instance;
    }

    void Game::run(sf::Vector2u windowSize,
                   sf::String   windowTitle,
                   ScenePtr     pScene)
    {
        _window.create(sf::VideoMode(windowSize), windowTitle);
        gameLoop();
    }

    void Game::setNextScene(ScenePtr pScene)
    {
        if (_settings.test(IMMEDIATE_SCENE_TRANSITION)) {
            _scenes.transitionScene(pScene);
            return;
        }
        if (_settings.test(SCENE_MULTISTACK) && _nextScene != nullptr) {
            _scenes.transitionScene(_nextScene);
            _nextScene = pScene;
        }
        _nextScene = pScene;
    }

    void Game::gameLoop()
    {
        while (true) {
            if (_nextScene != nullptr) {
                _scenes.transitionScene(_nextScene);
            }
            _clock.update();
            bool successful;
            _scenes.updateCurrentScene(_clock.getTime(), successful);
            if (!successful) {
                break;
            }
            while (std::optional event = _window.pollEvent()) {
                _scenes.handleEventWithScene(event, successful);
                if (!successful) {
                    break;
                }
            }
            if (!successful) {
                break;
            }
        }
    }

    void Game::setImmediateSceneTransferEnabled(bool enabled)
    {
        _settings.set(IMMEDIATE_SCENE_TRANSITION, enabled);
    }

    void Game::setSceneMultiStackEnabled(bool enabled)
    {
        _settings.set(SCENE_MULTISTACK, enabled);
    }
} // namespace libsatan::Engine
