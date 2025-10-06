#pragma once
#include "Graphics/Drawable.hpp"

namespace Graphics {
class Sprite : public Drawable {
    SDL_Texture* _texture;
    SDL_FRect    _source;
    SDL_FPoint   _position;
    SDL_FPoint   _origin;

public:
    void draw(SDL_Renderer* target) override;
    void setPosition(float xPos, float yPos);
    void setOrigin(float xPos, float yPos);
    void setTexture(SDL_Texture* texture, SDL_FRect sourceRect);
};
}
