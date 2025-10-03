#include "libcakes/Engine/Core.impl.hpp"

#include <SDL3/SDL_error.h>
#include <SDL3/SDL_init.h>
#include <SDL3/SDL_log.h>
#include <SDL3/SDL_render.h>
#include <SDL3_ttf/SDL_ttf.h>
namespace libcakes::Engine {
    void Core::impl::run(const char*     title,
                         int             width,
                         int             height,
                         SDL_WindowFlags flags,
                         ScenePtr        startingScene)
    {
        if (!SDL_Init(SDL_INIT_VIDEO)) {
            SDL_Log("Couldn't initialize SDL: %s", SDL_GetError());
            return;
        }
        if (!TTF_Init()) {
            SDL_Log("Couldn't initialize SDL_ttf: %s", SDL_GetError());
            return;
        }
        _pWindow = SDL_CreateWindow(title, width, height, flags);
        if (_pWindow == nullptr) {
            SDL_Log("Couldn't open the window: %s", SDL_GetError());
            return;
        }
        _pRenderer = SDL_CreateRenderer(_pWindow, nullptr);
        if (_pRenderer == nullptr) {
            SDL_Log("Couldn't initialize the renderer: %s", SDL_GetError());
            return;
        }
        if (_nextScene != nullptr) {
            transitionScene();
        }
        if (startingScene != nullptr) {
            _nextScene = startingScene;
            transitionScene();
        }
        if (_scenes.empty()) {
            SDL_Log("You can't have a game with no scenes!");
            return;
        }
        gameLoop();
    }

    void Core::impl::transitionScene()
    {
        if (!_scenes.empty())
            _scenes.top()->onBury();
        _nextScene->init();
        _scenes.push(_nextScene);
        _nextScene = nullptr;
    }

    void Core::impl::gameLoop()
    {
        // TODO: call the scene's update method
        // TODO: have the current scene handle events
        // TODO: draw the scene to the screen
    }

    void Core::impl::popScene()
    {
        if (_scenes.empty())
            return;
        _scenes.pop();
        if (_scenes.empty())
            return;
        _scenes.top()->onReveal();
    }

    void Core::impl::dumpCore()
    {
        while (!_scenes.empty())
            _scenes.pop();
    }

    void Core::impl::setBackgroundColor(Color color)
    {
        _backgroundColor = color;
    }

    void Core::impl::setNextScene(ScenePtr pScene)
    {
        _nextScene = pScene;
    }
    void Core::impl::enableSceneMultiset()
    {
        _settings.set(STACK_MULTIPLE_SCENES);
    }

    Core::impl::~impl() {}
} // namespace libcakes::Engine
