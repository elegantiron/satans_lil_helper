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

    const unsigned char satan_main[] = {
#embed "Images/Satan/main.png"
    };
    const unsigned char satan_eyes_open[] = {
#embed "Images/Satan/eyes_open.png"
    };
    const unsigned char satan_mouth_closed[] = {
#embed "Images/Satan/mouth_closed.png"
    };
}

namespace Assets {
    namespace Fonts {
        extern const sf::Font FairyDustB{fairy_dust, sizeof(fairy_dust)};
        extern const sf::Font CrayonLibre{crayon_libre, sizeof(crayon_libre)};
    }

    namespace Textures {
        namespace Satan {
            extern const sf::Texture Main{satan_main, sizeof(satan_main)};
            extern const sf::Texture EyesOpen{satan_eyes_open,
                                       sizeof(satan_eyes_open)};
            extern const sf::Texture MouthClosed{satan_mouth_closed,
                                          sizeof(satan_mouth_closed)};
        }

        namespace Items {}
    }
}

// NOLINTEND(*avoid-c-arrays,*array-to-pointer-decay)
