#pragma once
#include "libslh/System/GameTime.hpp"

#include <SFML/Graphics/Drawable.hpp>
#include <SFML/Window/Event.hpp>
#include <memory>

namespace libslh::Engine {
    enum class SceneResult : uint8_t
    {
        SUCCESS,
        CONTINUE,
        FAILURE
    };

    class Scene : public sf::Drawable {
    public:
        virtual void        init(){}
        virtual void        onBury(){}
        virtual void        onReveal(){}
        virtual SceneResult update(const GameTime& gametime)      = 0;
        virtual SceneResult event(std::optional<sf::Event> event) = 0;
    };

    using ScenePtr = std::shared_ptr<Scene>;
}
