#include "Engine/SceneManager.hpp"

#include <utility>

namespace SatansLilHelper::Engine {
    void SceneManager::handleEvent(std::optional<sf::Event> event,
                                   bool&                    successful,
                                   bool&                    keepRunning) {}

    void SceneManager::iterate(const GameTime& gameTime,
                               bool&           successful,
                               bool&           keepRunning) {
        if (_nextScene != nullptr) {
            transitionScene();
        }
        _scenes.top()->iterate(gameTime, successful, keepRunning);
        if (!successful) {
            return;
        }
        if (!keepRunning) {
            _scenes.pop();
            if (_scenes.empty()) {
                return;
            }
            keepRunning = true;
            _scenes.top()->onReveal();
        }
    }

    void SceneManager::transitionScene() {
        if (!_scenes.empty()) {
            _scenes.top()->onBury();
        }
        _nextScene->init();
        _scenes.push(std::move(_nextScene));
        _nextScene = nullptr;
    }

    void SceneManager::popScene(bool& nowEmpty) {
        _scenes.pop();
        if (_scenes.empty()) {
            nowEmpty = true;
            return;
        }
        _scenes.top()->onReveal();
    }
} // namespace Engine
