#include "libsatan/Engine/Game.hpp"

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
                   sf::String&  windowTitle,
                   ScenePtr     pScene)
    {
        _window.create(sf::VideoMode(windowSize), windowTitle);
        if (pScene != nullptr) {
            _scenes.transitionScene(pScene);
        }
        gameLoop();
    }

    void Game::setNextScene(ScenePtr& pScene)
    {
        if (_settings.test(
                static_cast<int>(GameSetting::IMMEDIATE_SCENE_TRANSITION))) {
            _scenes.transitionScene(pScene);
            return;
        }
        if (_settings.test(static_cast<int>(GameSetting::SCENE_MULTISTACK))
            && _nextScene != nullptr) {
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
            bool successful = false;
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

    void Game::updateSetting(GameSetting setting, bool enabled)
    {
        _settings.set(static_cast<int>(setting), enabled);
    }
    bool Game::getSetting(GameSetting setting)
    {
        return _settings.test(static_cast<int>(setting));
    }

    sf::Vector2u Game::getWindowSize() const
    {
        return _window.getSize();
    }
} // namespace libsatan::Engine
