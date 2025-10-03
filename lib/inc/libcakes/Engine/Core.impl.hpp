#pragma once
#include "libcakes/Clock.hpp"
#include "libcakes/Engine/Core.hpp"

#include <SDL3/SDL_render.h>
#include <SDL3/SDL_video.h>
#include <bitset>
#include <stack>

namespace libcakes::Engine {
/**
 * @brief Contains the implementation for the Core
 * 
 */
    class Core::impl {
        friend class Core;

        static constexpr int SAVE_ON_CLOSE         = 1;
        static constexpr int STACK_MULTIPLE_SCENES = 2;

        SDL_Window*          _pWindow   = nullptr;
        SDL_Renderer*        _pRenderer = nullptr;
        ScenePtr             _nextScene = nullptr;
        std::stack<ScenePtr> _scenes;
        Color                _backgroundColor;
        Clock                _clock;
        std::bitset<10>      _settings{0};

        /**
         * @brief Transition to the next Scene.
         * @details Handles initializing the scene, calling the current scene's onBury,
         * and clearing the pointer to the next scene.
         */
        void transitionScene();
        void popScene();
        void gameLoop();
        void dumpCore();
        void run(const char*     title,
                 int             width,
                 int             height,
                 SDL_WindowFlags flags,
                 ScenePtr        startingScene = nullptr);
        void setNextScene(ScenePtr pScene);
        void setBackgroundColor(Color color);
        void enableSceneMultiset();

    public:
        ~impl();
    };
}
