#pragma once
#include "libsatan/System/Clock.hpp"
#include "libsatan/Graphics/Color.hpp"
#include "libsatan/Engine/Scene.hpp"

#include <SDL3/SDL_video.h>
#include <bitset>
#include <stack>

namespace libsatan::Engine {

    /**
     * @brief Everything you need to manage a Game.
     *
     */
    class Core {
        static std::unique_ptr<Core> _instance;
        static constexpr int         SET_SIZE{10};

        enum Setting
        {
            MULTIPLE_SCENE_STACK,
            CLOSE_ON_ESCAPE
        };

        SDL_Window*           _window;
        SDL_Renderer*         _renderer;
        Color                 _backgroundColor;
        ScenePtr              _nextScene;
        std::stack<ScenePtr>  _scenes;
        std::bitset<SET_SIZE> _settings;
        Clock                 _clock;

        Core() {}
        void transitionScene();
        void popScene();
        void dumpCore();
        void gameLoop();
        bool initSDL(const char*     title,
                     int             width,
                     int             height,
                     SDL_WindowFlags flags);

    public:
        /**
         * @brief Get the Instance
         *
         * @return Core&
         */
        static Core& getInstance();

        /**
         * @brief Run the game
         *
         * @param title Title for the window
         * @param width Width of the window
         * @param height Height of the window
         * @param flags SDL_WindowFlags to use when creating the window
         * @param pScene The first Scene of the game
         */
        void run(const char*     title,
                 int             width,
                 int             height,
                 SDL_WindowFlags flags,
                 ScenePtr        pScene = nullptr);

        /**
         * @brief Set the Next Scene
         *
         * @param pScene
         */
        void setNextScene(ScenePtr pScene);

        /**
         * @brief Set the Background Color
         *
         * @param color
         */
        void setBackgroundColor(Color color);

        /**
         * @brief Enable setting multiple scenes at once
         *
         * @details With this setting enabled, setting the next scene with one
         * already set will immediately transition to the first scene set. Any
         * additional scenes added will cause an immediate transition to the
         * next scene.
         *
         * Without enabling this, setting the next scene when one is
         * already set will cause the previously set scene to be lost.
         */
        void enableSceneMultiset();

        /**
         * @brief Set whether the Core will exit when escape is pressed.
         *
         * @param value
         */
        void setExitOnEscape(bool value);
    };
} // namespace libsatan::Engine
