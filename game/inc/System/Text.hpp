#pragma once

namespace SatansLilHelper {
    class Text {
        SDL_Texture* _texture;
        const char*  _text;
        SDL_FRect    _bounds;

    public:
        Text(TTF_Font* font);

        void setOrigin(SDL_FPoint origin);
        void draw(SDL_Renderer* target) const;
    };
}
