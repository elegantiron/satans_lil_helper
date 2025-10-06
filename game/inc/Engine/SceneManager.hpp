#pragma once
#include "Engine/Scene.hpp"
#include "Utils/GameTime.hpp"

#include <SDL3/SDL.h>
#include <stack>

class SceneManager {
    std::stack<ScenePtr> _scenes;
    ScenePtr             _nextScene{nullptr};

    void transitionScene();

public:
    void handleEvent(SDL_Event* event, bool& successful, bool& keepRunning);
    void update(const GameTime& gameTime, bool& successful, bool& keepRunning);
    void draw();
    void setNextScene(ScenePtr pScene, bool immediate = false);
};
