#include "libsatan/Engine/Core.hpp"

#include <SDL3/SDL_error.h>
#include <SDL3/SDL_init.h>
#include <SDL3/SDL_log.h>
#include <SDL3_ttf/SDL_ttf.h>
namespace libsatan::Engine {
    SDL_Window*                 Core::_window{nullptr};
    SDL_Renderer*               Core::_renderer{nullptr};
    ScenePtr                    Core::_nextScene{nullptr};
    std::stack<ScenePtr>        Core::_scenes{};
    std::bitset<Core::SET_SIZE> Core::_settings{0};

    void Core::run(const char*     title,
                   int             width,
                   int             height,
                   SDL_WindowFlags flags,
                   ScenePtr        pScene)
    {
        if (!initSDL(title, width, height, flags))
            return;
        if (_nextScene != nullptr) {
            transitionScene();
        }
        if (pScene != nullptr) {
            _nextScene = pScene;
            transitionScene();
        }
        if (_scenes.empty()) {
            SDL_Log("You can't have a game without at least one scene!");
            return;
        }
        gameLoop();
    }

    void Core::transitionScene()
    {
        if (_scenes.empty())
            return;
        _scenes.top()->onBury();
        _nextScene->init();
        _scenes.push(_nextScene);
        _nextScene = nullptr;
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
            SDL_Log("Couldn't initialize renderer: %s", SDL_GetError());
            return false;
        }
        if (!TTF_Init()) {
            SDL_Log("Couldn't initialize SDL_ttf: %s", SDL_GetError());
            return false;
        }
        return true;
    }

    void Core::gameLoop()
    {
        if (_scenes.empty())
            return;
        switch (_scenes.top()->update(_clock.newFrame())) {
            using enum libsatan::Engine::SceneResult;
        case CONTINUE:
            break;
        case SUCCESS:
            popScene();
            if (_scenes.empty())
                return;
            break;
        case FAILURE:
            return;
        }
        SDL_Event event;
        while (SDL_PollEvent(&event)) {
            switch (_scenes.top()->event(&event)) {
                using enum libsatan::Engine::SceneResult;
            case CONTINUE:
                break;
            case SUCCESS:
                popScene();
                if (_scenes.empty())
                    return;
                break;
            case FAILURE:
                return;
            }
        }
    }
} // namespace libsatan::Engine
