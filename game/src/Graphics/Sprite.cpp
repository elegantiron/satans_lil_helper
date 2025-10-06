#include "Graphics/Sprite.hpp"

namespace Graphics {
void Sprite::draw(SDL_Renderer* target) {
    SDL_FRect dest;
    SDL_RenderTexture(target, _texture, &_source, &dest);
}

void Sprite::setPosition(float xPos, float yPos) {
    _position.x = xPos;
    _position.y = yPos;
}

void Sprite::setTexture(SDL_Texture* texture, SDL_FRect sourceRect) {
    _source  = sourceRect;
    _texture = texture;
}

void Sprite::setOrigin(float xPos, float yPos){
    _origin.x = xPos;
    _origin.y = yPos;
}
}
