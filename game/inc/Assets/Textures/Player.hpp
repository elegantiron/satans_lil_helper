#pragma once

namespace {
    const unsigned char player[] = {
#embed "../../../assets/Images/Player/player.png"
    };
}

namespace SatansLilHelper::Assets::Textures::Player {
    inline const sf::Texture Main{player, sizeof(player)};
}
