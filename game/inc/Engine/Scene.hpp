#pragma once
#include "GameTime.hpp"

#include <memory>

namespace SatansLilHelper::Engine {
    class Scene {
        friend class SceneManager;
        virtual void iterate(const GameTime& gameTime,
                             bool&           successful,
                             bool&           keepRunning)
            = 0;

        virtual void onReveal() {}

        virtual void onBury() {}

        virtual void init() {}

    public:
        virtual ~Scene() = default;
    };

    using ScenePtr = std::shared_ptr<Scene>;
}
