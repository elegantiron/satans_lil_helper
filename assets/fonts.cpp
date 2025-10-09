#pragma clang diagnostic ignored "-Wc23-extensions"
#include "SLHAssets.hpp"

// NOLINTBEGIN(*avoid-c-arrays,*array-to-pointer-decay)
namespace {
    const unsigned char fairy_dust[] = {
#embed "Fonts/FairyDustB.ttf"
    };

    const unsigned char crayon_libre[] = {
#embed "Fonts/CrayonLibre.ttf"
    };

}

namespace Assets {
    namespace Fonts {
        extern const sf::Font FairyDustB{fairy_dust, sizeof(fairy_dust)};
        extern const sf::Font CrayonLibre{crayon_libre, sizeof(crayon_libre)};
    }
}

// NOLINTEND(*avoid-c-arrays,*array-to-pointer-decay)
