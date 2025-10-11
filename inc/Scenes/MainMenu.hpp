#pragma once

#include "Engine/Scene.hpp"

#include "Menu.hpp"

namespace SatansLilHelper::Scenes {
    class MainMenu : public Engine::Scene {
        Menu _menu;

        void iterate(const GameTime& gameTime,
                     bool&           successful,
                     bool&           keepRunning) override;
        void handleEvent(std::optional<sf::Event> event,
                         bool&                    successful,
                         bool&                    keepRunning) override;
        void draw(sf::RenderTarget& target,
                  sf::RenderStates  states) const override;

    public:
        MainMenu(Engine::ScenePtr parent = nullptr);
        void addItem(const char* text);
    };
}
