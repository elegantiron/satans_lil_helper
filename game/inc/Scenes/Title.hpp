#pragma once

#include "Engine/Scene.hpp"

#include <SFML/Graphics/Font.hpp>
#include <SFML/Graphics/Text.hpp>

namespace SatansLilHelper::Scenes {
    class Title : public Engine::Scene {
        sf::Font _font;
        sf::Text _title;
        sf::Text _pressStart;

        void setTextPositions();
        void init() override;
        void draw(sf::RenderTarget& target,
                  sf::RenderStates  states) const override;

    public:
        Title();
    };
}
