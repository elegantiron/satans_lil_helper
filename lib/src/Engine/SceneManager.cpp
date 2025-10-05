#include "libslh/Engine/SceneManager.hpp"

#include <SFML/Graphics/RenderStates.hpp>
#include <SFML/Graphics/RenderTarget.hpp>
#include <utility>

namespace libslh::Engine {
    void SceneManager::updateScene(const GameTime& gameTime, bool& successful) {
        successful = true;
        if (_scenes.empty()) {
            successful = false;
            return;
        }
        switch (_scenes.top()->update(gameTime)) {
            using enum SceneResult;
        case SUCCESS:
            popScene();
            [[fallthrough]];
        case CONTINUE:
            return;
        case FAILURE:
            successful = false;
            return;
        }
    }

    void SceneManager::popScene() {
        if (_scenes.empty()) {
            return;
        }
        _scenes.pop();
        if (_scenes.empty()) {
            return;
        }
        _scenes.top()->onReveal();
    }

    void SceneManager::dispatchEvent(std::optional<sf::Event> event,
                                     bool& handledSuccessfully) {
        handledSuccessfully = true;
        if (_scenes.empty()) {
            handledSuccessfully = false;
            return;
        }
        switch (_scenes.top()->event(event)) {
            using enum SceneResult;
        case SUCCESS: {
            popScene();
            [[fallthrough]];
        }
        case CONTINUE:
            break;
        case FAILURE:
            handledSuccessfully = false;
            return;
        }
    }

    void SceneManager::init(ScenePtr pScene) {
        transitionScene(std::move(pScene));
    }

    void SceneManager::transitionScene(ScenePtr pScene) {
        if (!_scenes.empty()) {
            _scenes.top()->onBury();
        }
        pScene->init();
        _scenes.push(std::move(pScene));
    }

    bool SceneManager::hasScenes() const {
        return !_scenes.empty();
    }

    void SceneManager::draw(sf::RenderTarget& target,
                            sf::RenderStates  states) const {
        target.draw(*_scenes.top(), states);
    }
} // namespace libslh::Engine
