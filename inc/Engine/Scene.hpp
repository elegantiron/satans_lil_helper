#pragma once
#include "System/GameTime.hpp"

#include <SFML/Graphics/Drawable.hpp>
#include <SFML/Window/Event.hpp>
#include <memory>

namespace SatansLilHelper {
    class Scene;
    using ScenePtr = std::shared_ptr<Scene>;

    class Scene : public sf::Drawable {
    protected:
        ScenePtr _parent;

    public:
        Scene(ScenePtr);
        virtual ~Scene();
        virtual void iterate(const GameTime&, bool&, bool&)      = 0;
        virtual void handleEvent(const sf::Event&, bool&, bool&) = 0;
        virtual void init();
        virtual void onBury();
        virtual void onReveal();
    };
}
