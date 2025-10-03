#pragma once
#include "libsatan/Color.hpp"
#include "libsatan/Engine/Scene.hpp"

#include <SDL3/SDL_video.h>
#include <bitset>
#include <stack>

namespace libsatan::Engine {

    class Core {
        enum Setting
        {
            MULTIPLE_SCENE_STACK
        };
        static SDL_Window*          _window;
        static SDL_Renderer*        _renderer;
        static ScenePtr             _nextScene;
        static std::stack<ScenePtr> _scenes;
        static std::bitset<10>      _settings;

        Core() {}
        void transitionScene();
        void popScene();
        void dumpCore();
        void gameLoop();

    public:
        static void run(const char*     title,
                        int             width,
                        int             height,
                        SDL_WindowFlags flags,
                        ScenePtr        pScene = nullptr);
        static void setNextScene(ScenePtr pScene);
        static void setBackgroundColor(Color color);
        static void enableSceneMultiset();
    };
}
