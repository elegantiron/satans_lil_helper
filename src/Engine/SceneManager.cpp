#include "Engine/SceneManager.hpp"

#include <utility>

namespace SatansLilHelper::Engine {
    void SceneManager::handleEvent(std::optional<sf::Event> event,
                                   bool&                    successful,
                                   bool&                    keepRunning) {
        if (_scenes.empty()) {
            keepRunning = false;
            return;
        }
        _scenes.top()->handleEvent(event, successful, keepRunning);
        bool nowEmpty = true;
        if (!keepRunning) {
            popScene(nowEmpty);

            if (!nowEmpty) {
                keepRunning = true;
            }
        }
    }

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
        nowEmpty = false;
        _scenes.pop();
        if (_scenes.empty()) {
            nowEmpty = true;
            return;
        }
        _scenes.top()->onReveal();
    }

    void SceneManager::setNextScene(ScenePtr pScene) {
        _nextScene = std::move(pScene);
        if (_scenes.empty()) {
            transitionScene();
        }
    }

    void SceneManager::draw(sf::RenderTarget& target,
                            sf::RenderStates  states) const {
        target.draw(*_scenes.top(), states);
    }

    ScenePtr SceneManager::getCurrentScene() const {
        return _scenes.top();
    }
} // namespace SatansLilHelper::Engine
