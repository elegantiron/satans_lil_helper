#pragma once

#include "Engine/Scene.hpp"

namespace Scenes {
class Title : public Scene {
    void update(const GameTime& gameTime,
                bool&           successful,
                bool&           keepRuning) override;
    void handleEvent(SDL_Event* event,
                     bool&      successful,
                     bool&      keepRunning) override;
    void init() override;
    void onReveal() override;
};
}
