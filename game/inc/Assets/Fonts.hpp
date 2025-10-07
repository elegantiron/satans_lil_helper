#pragma once

namespace {
    const unsigned char fairy_dust_b[] = {
#embed "../../assets/Fonts/FairyDustB.ttf"
    };
    const unsigned char crayon_libre[] = {
#embed "../../assets/Fonts/CrayonLibre.ttf"
    };
}

namespace SatansLilHelper::Assets::Fonts {
    inline const sf::Font FairyDustB{fairy_dust_b, sizeof(fairy_dust_b)};
    inline const sf::Font CrayonLibre{crayon_libre, sizeof(crayon_libre)};
}
