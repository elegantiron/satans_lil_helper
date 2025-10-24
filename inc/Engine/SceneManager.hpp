#pragma once

#include "Engine/Scene.hpp"

#include <SFML/Graphics/Drawable.hpp>
#include <stack>

namespace SatansLilHelper {
    class SceneManager : public sf::Drawable {
        ScenePtr             _nextScene = nullptr;
        std::stack<ScenePtr> _scenes;

        void draw(sf::RenderTarget& target,
                  sf::RenderStates  states) const override;
        void setNextScene(ScenePtr nextScene);
        [[nodiscard]]
        ScenePtr getCurrentScene() const;
    };
}
