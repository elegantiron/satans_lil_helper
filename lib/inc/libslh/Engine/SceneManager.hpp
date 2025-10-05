#pragma once
#include "libslh/Engine/Scene.hpp"
#include "libslh/System/GameTime.hpp"

#include <SFML/Graphics/Drawable.hpp>
#include <SFML/Window/Event.hpp>
#include <optional>
#include <stack>

namespace libslh::Engine {
    class SceneManager : public sf::Drawable {
        std::stack<ScenePtr> _scenes;

        void popScene();

    public:
        [[nodiscard]] bool hasScenes() const;

        void draw(sf::RenderTarget& target,
                  sf::RenderStates  states) const override;
        void updateScene(const GameTime& gameTime, bool& successful);
        void init(ScenePtr pScene = nullptr);
        void transitionScene(ScenePtr pScene);
        void dispatchEvent(std::optional<sf::Event> event,
                           bool&                    handledSuccessfully);
    };
}
