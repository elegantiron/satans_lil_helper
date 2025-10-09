#pragma clang diagnostic ignored "-Wc23-extensions"
#include "SLHAssets.hpp"

// NOLINTBEGIN(*avoid-c-arrays,*array-to-pointer-decay)
namespace {
    const unsigned char satan_main[] = {
#embed "Images/Satan/main.png"
    };
    const unsigned char satan_eyes_closed[] = {
#embed "Images/Satan/eyes_closed.png"
    };
    const unsigned char satan_eyes_open[] = {
#embed "Images/Satan/eyes_open.png"
    };
    const unsigned char satan_mouth_closed[] = {
#embed "Images/Satan/mouth_closed.png"
    };
    const unsigned char satan_mouth_open[] = {
#embed "Images/Satan/mouth_open.png"
    };
}

namespace Assets::Textures::Satan {
    extern const sf::Texture Main{satan_main, sizeof(satan_main)};
    extern const sf::Texture EyesClosed{satan_eyes_closed,
                                        sizeof(satan_eyes_closed)};
    extern const sf::Texture EyesOpen{satan_eyes_open, sizeof(satan_eyes_open)};
    extern const sf::Texture MouthClosed{satan_mouth_closed,
                                         sizeof(satan_mouth_closed)};
    extern const sf::Texture MouthOpen{satan_mouth_open,
                                       sizeof(satan_mouth_open)};

}

// NOLINTEND(*avoid-c-arrays,*array-to-pointer-decay)
