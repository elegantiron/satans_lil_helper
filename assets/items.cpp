#pragma clang diagnostic ignored "-Wc23-extensions"
#include "SLHAssets.hpp"

// NOLINTBEGIN(*avoid-c-arrays,*array-to-pointer-decay)

namespace {
    const unsigned char sack[]{
#embed "Images/Items/sack.png"
    };
}

namespace Assets::Textures::Items {
    extern const sf::Texture Sack{sack, sizeof(sack)};
}

// NOLINTEND(*avoid-c-arrays,*array-to-pointer-decay)
