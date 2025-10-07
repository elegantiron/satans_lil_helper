#pragma once

namespace {
    const unsigned char satan_main[] = {
#embed "../../../assets/Images/Satan/main.png"
    };
    const unsigned char satan_eyes_closed[] = {
#embed "../../../assets/Images/Satan/eyes_closed.png"
    };
    const unsigned char satan_eyes_open[] = {
#embed "../../../assets/Images/Satan/eyes_open.png"
    };
    const unsigned char satan_mouth_closed[] = {
#embed "../../../assets/Images/Satan/mouth_closed.png"
    };
    const unsigned char satan_mouth_open[] = {
#embed "../../../assets/Images/Satan/mouth_open.png"
    };
}

namespace SatansLilHelper::Assets::Textures::Satan {
    inline const sf::Texture Main{satan_main, sizeof(satan_main)};
    inline const sf::Texture EyesOpen{satan_eyes_open, sizeof(satan_eyes_open)};
    inline const sf::Texture MouthClosed{satan_mouth_closed,
                                         sizeof(satan_mouth_closed)};
    inline const sf::Texture MouthOpen{satan_mouth_open,
                                       sizeof(satan_mouth_open)};
}
