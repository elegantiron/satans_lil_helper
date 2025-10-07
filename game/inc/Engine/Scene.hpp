#pragma once
#include "GameTime.hpp"

#include <SFML/Graphics/Drawable.hpp>
#include <SFML/Window/Event.hpp>
#include <memory>
#include <optional>

namespace SatansLilHelper::Engine {
    class Scene : public sf::Drawable {
        friend class SceneManager;
        virtual void iterate(const GameTime& gameTime,
                             bool&           successful,
                             bool&           keepRunning)
            = 0;
        virtual void handleEvent(std::optional<sf::Event> event,
                                 bool&                    successful,
                                 bool&                    keepRunning)
            = 0;

        virtual void onReveal() {}

        virtual void onBury() {}

        virtual void init() {}

    public:
        virtual ~Scene() = default;
    };

    using ScenePtr = std::shared_ptr<Scene>;
}
