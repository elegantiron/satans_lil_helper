#define SDL_MAIN_USE_CALLBACKS 1
#include "GameState.hpp"

#include <SDL3/SDL_main.h>

using namespace SatansLilHelper;

extern const Asset FairyDust;

SDL_AppResult SDL_AppInit(void** appstate, int /*argc*/, char* /*argv*/[])
{
    *appstate        = new GameState;
    GameState& state = *static_cast<GameState*>(*appstate);
    if (!SDL_Init(SDL_INIT_VIDEO)) {
        SDL_Log("Couldn't initialize SDL: %s", SDL_GetError());
        return SDL_APP_FAILURE;
    }

    state.window = SDL_CreateWindow(Constants::Title,
                                    Constants::Window::Width,
                                    Constants::Window::Height,
                                    0);
    if (state.window == nullptr) {
        SDL_Log("Couldn't open winodw: %s", SDL_GetError());
        return SDL_APP_FAILURE;
    }
    state.renderer = SDL_CreateRenderer(state.window, nullptr);
    if (state.renderer == nullptr) {
        SDL_Log("Couldn't initialize renderer: %s", SDL_GetError());
        return SDL_APP_FAILURE;
    }
    if (!TTF_Init()) {
        SDL_Log("Couldn't initialize the TTF library: %s", SDL_GetError());
        return SDL_APP_FAILURE;
    }
    state.font
        = TTF_OpenFontIO(SDL_IOFromConstMem(FairyDust.data, FairyDust.size),
                         true,
                         18.0F);

    SDL_Surface* text = TTF_RenderText_Blended(state.font,
                                               "Satan's Lil Helper",
                                               0,
                                               {255, 255, 255, 255});
    state.texture     = SDL_CreateTextureFromSurface(state.renderer, text);
    SDL_DestroySurface(text);

    return SDL_APP_CONTINUE;
}

SDL_AppResult SDL_AppIterate(void* appstate)
{
    GameState& state = *static_cast<GameState*>(appstate);
    // NOLINTNEXTLINE
    uint8_t r, g, b, a;
    SDL_GetRenderDrawColor(state.renderer, &r, &g, &b, &a);
    SDL_SetRenderDrawColor(state.renderer, 0, 0, 0, SDL_ALPHA_OPAQUE);
    SDL_RenderClear(state.renderer);

    SDL_SetRenderDrawColor(state.renderer, r, g, b, a);
    SDL_RenderPresent(state.renderer);
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

void SDL_AppQuit(void* appstate, SDL_AppResult /*result*/)
{
    auto* state = static_cast<GameState*>(appstate);
    delete state;
    TTF_Quit();
}
