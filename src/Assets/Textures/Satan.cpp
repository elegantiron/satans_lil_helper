#include "Assets/Textures/Satan.hpp"

// NOLINTBEGIN(*c-arrays)
namespace {
    const unsigned char main[] = {
#embed "../../../assets/Images/Satan/main.png"
    };
    const unsigned char eyes_closed[] = {
#embed "../../../assets/Images/Satan/eyes_closed.png"
    };
    const unsigned char eyes_open[] = {
#embed "../../../assets/Images/Satan/eyes_open.png"
    };
    const unsigned char mouth_closed[] = {
#embed "../../../assets/Images/Satan/mouth_closed.png"
    };
    const unsigned char mouth_open[] = {
#embed "../../../assets/Images/Satan/mouth_open.png"
    };
}

// NOLINTEND(*c-arrays)
// NOLINTBEGIN(*pointer-decay)
namespace SatansLilHelper::Assets::Textures::Satan {
    extern const sf::Texture Main{main, sizeof(main)};
    extern const sf::Texture EyesClosed{eyes_closed, sizeof(eyes_closed)};
    extern const sf::Texture EyesOpen{eyes_open, sizeof(eyes_open)};
    extern const sf::Texture MouthClosed{mouth_closed, sizeof(mouth_closed)};
    extern const sf::Texture MouthOpen{mouth_open, sizeof(mouth_open)};
}

// NOLINTEND(*pointer-decay)
