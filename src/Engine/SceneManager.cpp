#include "Engine/SceneManager.hpp"

#include <SFML/Graphics/RenderStates.hpp>
#include <SFML/Graphics/RenderTarget.hpp>
#include <utility>

namespace SatansLilHelper {
    void SceneManager::draw(sf::RenderTarget& target,
                            sf::RenderStates  states) const {
        target.draw(*_scenes.top(), states);
    }

    void SceneManager::setNextScene(ScenePtr nextScene) {
        _nextScene = std::move(nextScene);
    }

    ScenePtr SceneManager::getCurrentScene() const {
        return _scenes.top();
    }
    
}
