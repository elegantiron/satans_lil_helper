#define SDL_MAIN_USE_CALLBACKS 1
#include "GameState.hpp"

#include <SDL3/SDL_main.h>
using namespace SatansLilHelper;

SDL_AppResult SDL_AppInit(void** appstate, int /*argc*/, char* /*argv*/[])
{
    *appstate        = new GameState;
    GameState& state = *static_cast<GameState*>(*appstate);
    if (!SDL_Init(SDL_INIT_VIDEO)) {
        SDL_Log("Couldn't initialize SDL: %s", SDL_GetError());
        return SDL_APP_FAILURE;
    }

    state.Window = SDL_CreateWindow(Constants::Title,
                                    Constants::Window::Width,
                                    Constants::Window::Height,
                                    0);
    if (state.Window == nullptr) {
        SDL_Log("Couldn't open winodw: %s", SDL_GetError());
        return SDL_APP_FAILURE;
    }
    state.Renderer = SDL_CreateRenderer(state.Window, nullptr);
    if (state.Renderer == nullptr) {
        SDL_Log("Couldn't initialize renderer: %s", SDL_GetError());
        return SDL_APP_FAILURE;
    }
    if (!TTF_Init()) {
        SDL_Log("Couldn't initialize the TTF library: %s", SDL_GetError());
        return SDL_APP_FAILURE;
    }

    return SDL_APP_CONTINUE;
}

SDL_AppResult SDL_AppIterate(void* appstate)
{
    GameState& state = *static_cast<GameState*>(appstate);
    // NOLINTNEXTLINE
    uint8_t r, g, b, a;
    SDL_GetRenderDrawColor(state.Renderer, &r, &g, &b, &a);
    SDL_SetRenderDrawColor(state.Renderer, 0, 0, 0, SDL_ALPHA_OPAQUE);
    SDL_RenderClear(state.Renderer);
    SDL_SetRenderDrawColor(state.Renderer, r, g, b, a);
    SDL_RenderPresent(state.Renderer);
    return SDL_APP_CONTINUE;
}

SDL_AppResult SDL_AppEvent(void* appstate, SDL_Event* event)
{
    if (event->type == SDL_EVENT_QUIT) {
        return SDL_APP_SUCCESS;
    }
    if (event->type == SDL_EVENT_KEY_DOWN) {
        if (event->key.scancode == SDL_SCANCODE_ESCAPE) {
            return SDL_APP_SUCCESS;
        }
    }
    return SDL_APP_CONTINUE;
}

void SDL_AppQuit(void* appstate, SDL_AppResult result)
{
    auto* state = static_cast<GameState*>(appstate);
    delete state;
    TTF_Quit();
}
