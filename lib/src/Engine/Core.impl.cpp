#include "libcakes/Engine/Core.impl.hpp"

#include <SDL3/SDL_error.h>
#include <SDL3/SDL_init.h>
#include <SDL3/SDL_log.h>
#include <SDL3/SDL_render.h>

namespace libcakes::Engine {
    void Core::impl::run(const char*     title,
                         int             width,
                         int             height,
                         SDL_WindowFlags flags,
                         ScenePtr        startingScene) {
        if (!SDL_Init(SDL_INIT_VIDEO)) {
            SDL_Log("Couldn't initialize SDL: %s", SDL_GetError());
            return;
        }
        _pWindow=SDL_CreateWindow(title, width, height, flags);
        if (_pWindow == nullptr) {
            SDL_Log("Couldn't open the window: %s", SDL_GetError());
            return;
        }
        _pRenderer=SDL_CreateRenderer(_pWindow, nullptr);
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
        if(_scenes.empty()){
            SDL_Log("You can't have a game with no scenes!");
            return;
        }
    }

    void Core::impl::transitionScene(){
        if (!_scenes.empty())
        _scenes.top()->onBury();
    _nextScene->init();
    _scenes.push(_nextScene);
    _nextScene=nullptr;
    }
}
