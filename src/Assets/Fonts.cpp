#include "Assets/Fonts.hpp"

// NOLINTBEGIN(*c-arrays,*pointer-decay)
namespace {
    const unsigned char fairy_dust[] = {
#embed "../../assets/Fonts/FairyDustB.ttf"
    };
    const unsigned char crayon_libre[] = {
#embed "../../assets/Fonts/CrayonLibre.ttf"
    };
    constexpr int   MENU_CHARACTER_HEIGHT      = 55;
    constexpr float DEFAULT_OUTLINE_THICKNESS  = 0.0F;
    constexpr float SELECTED_OUTLINE_THICKNESS = 0.0F;
}

namespace SatansLilHelper::Assets::Fonts {
    extern const sf::Font FairyDustB{fairy_dust, sizeof(fairy_dust)};
    extern const sf::Font CrayonLibre{crayon_libre, sizeof(crayon_libre)};

    namespace Menu {
        extern const libslh::Font Default{CrayonLibre,
                                          MENU_CHARACTER_HEIGHT,
                                          sf::Color::White,
                                          DEFAULT_OUTLINE_THICKNESS,
                                          sf::Color::Black,
                                          sf::Text::Regular};
        extern const libslh::Font Selected{CrayonLibre,
                                           MENU_CHARACTER_HEIGHT,
                                           sf::Color::Red,
                                           SELECTED_OUTLINE_THICKNESS,
                                           sf::Color::Black,
                                           sf::Text::Regular};
    }
}

// NOLINTEND(*c-arrays,*pointer-decay)
