#pragma once

#include "Engine/Scene.hpp"

namespace SatansLilHelper::Scenes {
    class MainMenu : public Engine::Scene {
        void iterate(const GameTime& gameTime,
                     bool&           successful,
                     bool&           keepRunning) override;
        void handleEvent(std::optional<sf::Event> event,
                         bool&                    successful,
                         bool&                    keepRunning) override;
        void draw(sf::RenderTarget& target,
                  sf::RenderStates  states) const override;
    };
}
