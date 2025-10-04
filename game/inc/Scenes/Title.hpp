#pragma once

#include <SFML/Graphics/Font.hpp>
#include <SFML/Graphics/Text.hpp>
#include <SatanAssets.h>
#include <libsatan/Engine/Scene.hpp>

extern Asset FairyDust;

namespace SatansLilHelper {
    using SceneResult = libsatan::Engine::SceneResult;
    using GameTime    = libsatan::System::GameTime;

    class Title : public libsatan::Engine::Scene {
        sf::Font _font;
        sf::Text _title;
        sf::Text _pressStart;

    public:
        Title();

    private:
        SceneResult update(const GameTime& gameTime) override;
        SceneResult event(std::optional<sf::Event> event) override;
        void        draw(sf::RenderTarget& target,
                         sf::RenderStates  states) const override;
    };
}
