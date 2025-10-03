#pragma once
#include "libsatan/Clock.hpp"
#include "libsatan/Color.hpp"
#include "libsatan/Engine/Scene.hpp"

#include <SDL3/SDL_video.h>
#include <bitset>
#include <stack>


namespace libsatan::Engine {

    class Core {
        static constexpr int SET_SIZE{10};
        enum Setting
        {
            MULTIPLE_SCENE_STACK
        };
        static SDL_Window*           _window;
        static SDL_Renderer*         _renderer;
        static ScenePtr              _nextScene;
        static std::stack<ScenePtr>  _scenes;
        static std::bitset<SET_SIZE> _settings;
        static Clock                 _clock;

        Core() {}
        static void transitionScene();
        static void popScene();
        static void dumpCore();
        static void gameLoop();
        static bool initSDL(const char*     title,
                            int             width,
                            int             height,
                            SDL_WindowFlags flags);

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
} // namespace libsatan::Engine
