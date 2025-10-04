#ifndef LIBSATAN_ENGINE_SCENE_HPP
#define LIBSATAN_ENGINE_SCENE_HPP

#include "libsatan/System/GameTime.hpp"

#include <SFML/Graphics/Drawable.hpp>
#include <SFML/Window/Event.hpp>
#include <memory>

namespace libsatan::Engine {
    using namespace System;
    enum class SceneResult
    {
        SUCCESS,
        CONTINUE,
        FAILURE
    };

    class Scene : public sf::Drawable {
        friend class SceneStack;
        virtual SceneResult update(const GameTime& gameTime) = 0;
        virtual SceneResult event(std::optional<sf::Event>)  = 0;
        virtual void        init();
        virtual void        onBury();
        virtual void        onReveal();
    };

    using ScenePtr = std::shared_ptr<Scene>;
}

#endif
