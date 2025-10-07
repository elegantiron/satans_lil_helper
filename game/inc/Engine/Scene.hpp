#pragma once
#include "GameTime.hpp"

#include <SFML/Graphics/Drawable.hpp>
#include <SFML/Window/Event.hpp>
#include <memory>
#include <optional>
#include <utility>

namespace SatansLilHelper::Engine {
    class Scene;
    using ScenePtr = std::shared_ptr<Scene>;

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

    protected:
        ScenePtr _parent;

    public:
        virtual ~Scene() = default;

        Scene(ScenePtr parent) : _parent(std::move(parent)) {}
    };

}
