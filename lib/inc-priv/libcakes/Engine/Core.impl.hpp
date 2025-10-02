#pragma once
#include "libcakes/Engine/Core.hpp"

#include <SDL3/SDL_render.h>
#include <SDL3/SDL_video.h>
#include <stack>

namespace libcakes::Engine {
    class Core::impl {
        SDL_Window*          _pWindow    = nullptr;
        SDL_Renderer*        _pRenderer  = nullptr;
        ScenePtr             _nextScene = nullptr;
        std::stack<ScenePtr> _scenes;
        bool                 _saveOnClose = false;

        void transitionScene();
        void popScene();
        void gameLoop();

    public:
        void run(const char*     title,
                 int             width,
                 int             height,
                 SDL_WindowFlags flags,
                 ScenePtr        startingScene = nullptr);
        void setNextScene(ScenePtr pScene);
    };
}
