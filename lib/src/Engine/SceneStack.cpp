#include "libsatan/Engine/SceneStack.hpp"

namespace libsatan::Engine {
    void SceneStack::updateCurrentScene(const GameTime& gameTime,
                                        bool&           successful)
    {
        successful = true;
        switch (_scenes.top()->update(gameTime)) {
            using enum SceneResult;
        case CONTINUE:
            break;
        case SUCCESS:
            popScene();
            break;
        case FAILURE:
            successful = false;
            return;
        }
    }

    void SceneStack::handleEventWithScene(std::optional<sf::Event> event,
                                          bool&                    successful)
    {
        successful = true;
        if (_scenes.empty()) {
            return;
        }
        switch (_scenes.top()->event(event)) {
            using enum SceneResult;
        case CONTINUE:
            break;
        case SUCCESS:
            popScene();
            break;
        case FAILURE:
            successful = false;
            return;
        }
    }

    void SceneStack::popScene()
    {
        _scenes.pop();
        if (_scenes.empty()) {
            return;
        }
        _scenes.top()->onReveal();
    }

    void SceneStack::transitionScene(const ScenePtr& nextScene)
    {
        if (!_scenes.empty()) {
            _scenes.top()->onBury();
        }
        nextScene->init();
        _scenes.push(nextScene);
    }

    bool SceneStack::empty() const{
        return _scenes.empty();
    }
} // namespace libsatan::Engine
