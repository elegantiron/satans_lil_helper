#include "libsatan/Engine/Game.hpp"

#include <SFML/Window/VideoMode.hpp>
#include <iostream>

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
        std::cout << "creating window \n";
        _winMan.init(windowSize, windowTitle);
        if (pScene != nullptr) {
            _sceneMan.transitionScene(pScene);
        }
        std::cout << "enterring game loop\n";
        gameLoop();
    }

    void Game::setNextScene(ScenePtr& pScene)
    {
        if (_settings.test(
                static_cast<int>(GameSetting::IMMEDIATE_SCENE_TRANSITION))) {
            _sceneMan.transitionScene(pScene);
            return;
        }
        if (_settings.test(static_cast<int>(GameSetting::SCENE_MULTISTACK))
            && _nextScene != nullptr) {
            _sceneMan.transitionScene(_nextScene);
            _nextScene = pScene;
        }
        _nextScene = pScene;
    }

    void Game::gameLoop()
    {
        while (true) {
            if (_nextScene != nullptr) {
                _sceneMan.transitionScene(_nextScene);
            }
            _clock.update();
            bool successful = false;
            _sceneMan.updateCurrentScene(_clock.getTime(), successful);
            if (!successful) {
                break;
            }
            bool hasScenes = false;
            _winMan.handleEvents(_sceneMan, hasScenes);
            if (!hasScenes) {
                break;
            }

            _winMan.draw(_sceneMan);
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
        return _winMan.getWindowSize();
    }
} // namespace libsatan::Engine
