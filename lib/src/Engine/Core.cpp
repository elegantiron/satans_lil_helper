#include "libsatan/Engine/Core.hpp"

#include <SDL3/SDL_error.h>
#include <SDL3/SDL_init.h>
#include <SDL3/SDL_log.h>
#include <SDL3_ttf/SDL_ttf.h>
namespace libsatan::Engine {
    std::unique_ptr<Core> Core::_instance{nullptr};

    Core& Core::getInstance()
    {
        if (_instance == nullptr) {
            _instance.reset(new Core());
        }
        return *_instance;
    }

    void Core::transitionScene()
    {
        if (!_scenes.empty())
            _scenes.top()->onBury();
        _nextScene->init();
        _scenes.push(_nextScene);
    }

    void Core::popScene()
    {
        _scenes.pop();
        if (_scenes.empty())
            return;
        _scenes.top()->onReveal();
    }

    void Core::dumpCore()
    {
        while (!_scenes.empty()) {
            _scenes.pop();
        }
    }

    bool Core::initSDL(const char*     title,
                       int             width,
                       int             height,
                       SDL_WindowFlags flags)
    {
        if (!SDL_Init(SDL_INIT_VIDEO)) {
            SDL_Log("Couldn't initialize SDL: %s", SDL_GetError());
            return false;
        }
        _window = SDL_CreateWindow(title, width, height, flags);
        if (_window == nullptr) {
            SDL_Log("Couldn't create a window: %s", SDL_GetError());
            return false;
        }
        _renderer = SDL_CreateRenderer(_window, nullptr);
        if (_renderer == nullptr) {
            SDL_Log("Couldn't initialize the renderer: %s", SDL_GetError());
            return false;
        }
        if (!TTF_Init()) {
            SDL_Log("Couldnt initialzie SDL_ttf: %s", SDL_GetError());
            return false;
        }
        return true;
    }

    void Core::setNextScene(ScenePtr pScene)
    {
        if (_settings.test(MULTIPLE_SCENE_STACK) && _nextScene != nullptr) {
            transitionScene();
        }
        _nextScene = pScene;
    }

    void Core::setBackgroundColor(Color color)
    {
        _backgroundColor = color;
    }

    void Core::enableSceneMultiset()
    {
        _settings.set(MULTIPLE_SCENE_STACK);
    }

    void Core::run(const char*     title,
                   int             width,
                   int             height,
                   SDL_WindowFlags flags,
                   ScenePtr        pScene)
    {
        initSDL(title, width, height, flags);
        if (_nextScene != nullptr) {
            transitionScene();
        }
        if (pScene != nullptr) {
            _nextScene = pScene;
            transitionScene();
        }
        gameLoop();
    }

    void Core::setExitOnEscape(bool value)
    {
        _settings.set(CLOSE_ON_ESCAPE, value);
    }
} // namespace libsatan::Engine
