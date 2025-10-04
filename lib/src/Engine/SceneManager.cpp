#include "libsatan/Engine/SceneManager.hpp"

#include <SFML/Graphics/RenderStates.hpp>
#include <SFML/Graphics/RenderTarget.hpp>

namespace libsatan::Engine {
    void SceneManager::updateCurrentScene(const GameTime& gameTime,
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

    void SceneManager::handleEventWithScene(std::optional<sf::Event> event,
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

    void SceneManager::popScene()
    {
        _scenes.pop();
        if (_scenes.empty()) {
            return;
        }
        _scenes.top()->onReveal();
    }

    void SceneManager::transitionScene(const ScenePtr& nextScene)
    {
        if (!_scenes.empty()) {
            _scenes.top()->onBury();
        }
        nextScene->init();
        _scenes.push(nextScene);
    }

    bool SceneManager::empty() const
    {
        return _scenes.empty();
    }

    void SceneManager::draw(sf::RenderTarget& target,
                            sf::RenderStates   /*states*/) const
    {
        target.draw(*_scenes.top());
    }
} // namespace libsatan::Engine
