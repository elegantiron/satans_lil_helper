#pragma once

namespace SatansLilHelper{
    struct GameState{
        SDL_Window* window;
        SDL_Renderer* renderer;
        TTF_Font* font;
        SDL_Texture* texture;
    };
}