#pragma once
#include <SDL3/SDL_render.h>

namespace Graphics {
class Drawable {
public:
    virtual void draw(SDL_Renderer* target) = 0;
    virtual ~Drawable()                     = default;
};
}
