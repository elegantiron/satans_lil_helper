#include "libsatan/Engine/Core.hpp"

#include <SDL3/SDL_error.h>
#include <SDL3/SDL_init.h>
#include <SDL3/SDL_log.h>

namespace libsatan::Engine {
    void Core::run(const char*     title,
                   int             width,
                   int             height,
                   SDL_WindowFlags flags,
                   ScenePtr        pScene)
    {
        if (!SDL_Init(SDL_INIT_VIDEO)) {
            SDL_Log("Couldn't initialize SDL: %s", SDL_GetError());
            return;
        }
        _window = SDL_CreateWindow(title, width, height, flags);
        if (_window == nullptr) {
            SDL_Log("Couldn't create a window: %s", SDL_GetError());
            return;
        }
        _renderer = SDL_CreateRenderer(_window, nullptr);
        if (_renderer == nullptr) {
            SDL_Log("Couldn't initialize renderer: %s", SDL_GetError());
            return;
        }
    }
}
