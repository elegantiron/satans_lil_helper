#pragma once
#include "libcakes/GameTime.hpp"

#include <SDL3/SDL.h>
#include <memory>
namespace libcakes::Engine {

    enum class SceneResult {
        SUCCESS,
        CONTINUE,
        FAILURE
    };

    class Scene {
        friend class Core;

    protected:
        virtual SceneResult event(SDL_Event* event)          = 0;
        virtual SceneResult update(const GameTime& gameTime) = 0;
        virtual ~Scene()                                     = default;
        virtual void init() {}
        virtual void onBury() {}
        virtual void onReveal() {}
    };
    using ScenePtr = std::shared_ptr<Scene>;
}
