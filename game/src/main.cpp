#define SDL_MAIN_USE_CALLBACKS 1
#include "Engine/Core.hpp"
#include "Scenes/Title.hpp"

#include <SDL3/SDL.h>
#include <SDL3/SDL_main.h>

SDL_AppResult SDL_AppInit(void** appstate, int argc, char* argv[]) {
    Core& core       = Core::getInstance();
    bool  successful = false;
    core.init(successful);
    if (!successful) {
        return SDL_APP_FAILURE;
    }
    std::shared_ptr<Scenes::Title> pTitle{std::make_shared<Scenes::Title>()};
    core.setNextScene(pTitle);
    return SDL_APP_CONTINUE;
}

SDL_AppResult SDL_AppIterate(void* appstate) {
    Core& core        = Core::getInstance();
    bool  successful  = false;
    bool  keepRunning = false;
    core.iterate(successful, keepRunning);
    if (!successful) {
        return SDL_APP_FAILURE;
    }
    return keepRunning ? SDL_APP_CONTINUE : SDL_APP_SUCCESS;
}

SDL_AppResult SDL_AppEvent(void* appstate, SDL_Event* event) {
    Core& core         = Core::getInstance();
    bool  successful   = false;
    bool  iterateAgain = false;
    core.handleEvent(event, successful, iterateAgain);
    if (!successful) {
        return SDL_APP_FAILURE;
    }
    return iterateAgain ? SDL_APP_CONTINUE : SDL_APP_SUCCESS;
}

void SDL_AppQuit(void* appstate, SDL_AppResult result) {
}
