#include "Assets/Fonts.hpp"

// NOLINTBEGIN(*c-arrays,*pointer-decay)
namespace {
    const unsigned char fairy_dust[] = {
#embed "../../assets/Fonts/FairyDustB.ttf"
    };
    const unsigned char crayon_libre[] = {
#embed "../../assets/Fonts/CrayonLibre.ttf"
    };
}

namespace SatansLilHelper::Assets::Fonts {
    extern const sf::Font FairyDustB{fairy_dust, sizeof(fairy_dust)};
    extern const sf::Font CrayonLibre{crayon_libre, sizeof(crayon_libre)};
}

// NOLINTEND(*c-arrays,*pointer-decay)
