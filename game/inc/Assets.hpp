#pragma once
#include <cstddef>

// NOLINTBEGIN(cppcoreguidelines*)
namespace SatansLilHelper {
    struct Asset {
        unsigned char* data;
        std::size_t    size;
    };
}

namespace {
    const unsigned char fairy_dust_b[] = {
#embed "../assets/Fonts/FairyDustB.ttf"
    };
    const unsigned char crayon_libre[] = {
#embed "../assets/Fonts/CrayonLibre.ttf"
    };
    const unsigned char satan_main[] = {
#embed "../assets/Images/Satan/main.png"
    };
    const unsigned char satan_eyes_open[] = {
#embed "../assets/Images/Satan/eyes_open.png"
    };
    const unsigned char satan_mouth_closed[] = {
#embed "../assets/Images/Satan/mouth_closed.png"
    };
}

namespace SatansLilHelper::Assets {
    namespace Fonts {
        inline const sf::Font FairyDustB{fairy_dust_b, sizeof(fairy_dust_b)};
        inline const sf::Font CrayonLibre{crayon_libre, sizeof(crayon_libre)};
    }

    namespace Textures {
        namespace Satan {
            inline const sf::Texture Main{satan_main, sizeof(satan_main)};
            inline const sf::Texture EyesOpen{satan_eyes_open,
                                              sizeof(satan_eyes_open)};
            inline const sf::Texture MouthClosed{satan_mouth_closed,
                                                 sizeof(satan_mouth_closed)};
        }
    }
}

// NOLINTEND(cppcoreguidelines*)
