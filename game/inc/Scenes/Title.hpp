#pragma once

#include <libsatan/Engine/Scene.hpp>

namespace SatansLilHelper {
    using SceneResult = libsatan::Engine::SceneResult;
    using GameTime    = libsatan::System::GameTime;

    class Title : public libsatan::Engine::Scene {
        SceneResult update(const GameTime& gameTime) override;
        SceneResult event(std::optional<sf::Event> event) override;
        void        draw(sf::RenderTarget& target,
                         sf::RenderStates  states) const override;
    };
}
