#pragma once
#include "Utils/GameTime.hpp"

#include <SDL3/SDL_events.h>
#include <memory>

class Scene {
protected:
    friend class SceneManager;

    virtual void init() {}

    virtual void onBury() {}

    virtual void onReveal() {}

    virtual void update(const GameTime& gameTime,
                        bool&           successful,
                        bool&           keepRunning)
        = 0;
    virtual void handleEvent(SDL_Event* event,
                             bool&      successful,
                             bool&      keepRunning)
        = 0;

    virtual void draw() {}

public:
    Scene()                        = default;
    virtual ~Scene()               = default;
    Scene(Scene&&)                 = default;
    Scene(Scene&)                  = default;
    Scene& operator=(Scene const&) = default;
    Scene& operator=(Scene&&)      = default;
};

using ScenePtr = std::shared_ptr<Scene>;
